﻿//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
//
//*********************************************************

using System;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Devices.Enumeration;
using Windows.Devices.Usb;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace Windows10.Common
{
    /// <summary>
    /// The purpose of this class is to demonstrate what to do to a UsbDevice when a specific app event
    /// is raised (app suspension and resume) or when the device is disconnected. In addition to handling
    /// the UsbDevice, the app's state should also be saved upon app suspension (will not be demonstrated here).
    /// 
    /// This class will also demonstrate how to handle device watcher events.
    /// 
    /// For simplicity, this class will only allow at most one device to be connected at any given time. In order
    /// to make this class support multiple devices, make this class a non-singleton and create multiple instances
    /// of this class; each instance should watch one connected device.
    /// </summary>
    public class EventHandlerForDevice
    {
        /// <summary>
        /// Allows for singleton EventHandlerForDevice
        /// </summary>
        private static volatile EventHandlerForDevice eventHandlerForDevice;

        /// <summary>
        /// Used to synchronize threads to avoid multiple instantiations of eventHandlerForDevice.
        /// </summary>
        private static Object singletonCreationLock = new Object();

        private String deviceSelector;
        private DeviceWatcher deviceWatcher;

        private DeviceInformation deviceInformation;
        private DeviceAccessInformation deviceAccessInformation;
        private UsbDevice device;

        private SuspendingEventHandler appSuspendCallback;

        private SuspendingEventHandler appSuspendEventHandler;
        private EventHandler<Object> appResumeEventHandler;

        private TypedEventHandler<EventHandlerForDevice, DeviceInformation> deviceCloseCallback;
        private TypedEventHandler<EventHandlerForDevice, DeviceInformation> deviceConnectedCallback;

        private TypedEventHandler<DeviceWatcher, DeviceInformation> deviceAddedEventHandler;
        private TypedEventHandler<DeviceWatcher, DeviceInformationUpdate> deviceRemovedEventHandler;
        private TypedEventHandler<DeviceAccessInformation, DeviceAccessChangedEventArgs> deviceAccessEventHandler;

        private Boolean watcherSuspended;
        private Boolean watcherStarted;

        private Boolean isBackgroundTask;
        private Boolean isEnabledAutoReconnect;

        // A pointer back to the main page.  This is needed if you want to call methods in MainPage such
        // as NotifyUser()
        private MainPage rootPage = MainPage.Current;

        /// <summary>
        /// Enforces the singleton pattern so that there is only one object handling app events
        /// as it relates to the UsbDevice because this sample app only supports communicating with one device at a time. 
        ///
        /// An instance of EventHandlerForDevice is globally available because the device needs to persist across scenario pages.
        ///
        /// If there is no instance of EventHandlerForDevice created before this property is called,
        /// an EventHandlerForDevice will be created; the EventHandlerForDevice created this way
        /// is not meant for BackgroundTasks.
        /// </summary>
        public static EventHandlerForDevice Current
        {
            get
            {
                lock (singletonCreationLock)
                {
                    if (eventHandlerForDevice == null)
                    {
                        CreateNewEventHandlerForDevice();
                    }
                }

                return eventHandlerForDevice;
            }
        }

        /// <summary>
        /// Creates a new instance of EventHandlerForDevice, enables auto reconnect, and uses it as the Current instance.
        /// </summary>
        public static void CreateNewEventHandlerForDevice()
        {
            eventHandlerForDevice = new EventHandlerForDevice(false);
        }

        /// <summary>
        /// Creates a new instance of EventHandlerForDevice, disables auto reconnect, and uses it as the Current instance.
        /// Background tasks do not need to worry about app events, so we will not be registering for app events
        /// </summary>
        public static void CreateNewEventHandlerForDeviceForBackgroundTasks()
        {
            eventHandlerForDevice = new EventHandlerForDevice(true);
        }

        public SuspendingEventHandler OnAppSuspendCallback
        {
            get
            {
                return appSuspendCallback;
            }

            set
            {
                appSuspendCallback = value;
            }
        }

        public TypedEventHandler<EventHandlerForDevice, DeviceInformation> OnDeviceClose
        {
            get
            {
                return deviceCloseCallback;
            }

            set
            {
                deviceCloseCallback = value;
            }
        }

        public TypedEventHandler<EventHandlerForDevice, DeviceInformation> OnDeviceConnected
        {
            get
            {
                return deviceConnectedCallback;
            }

            set
            {
                deviceConnectedCallback = value;
            }
        }

        public Boolean IsDeviceConnected
        {
            get
            {
                return (device != null);
            }
        }

        public UsbDevice Device
        {
            get
            {
                return device;
            }
        }

        /// <summary>
        /// This DeviceInformation represents which device is connected or which device will be reconnected when
        /// the device is plugged in again (if IsEnabledAutoReconnect is true);.
        /// </summary>
        public DeviceInformation DeviceInformation
        {
            get
            {
                return deviceInformation;
            }
        }

        /// <summary>
        /// Returns DeviceAccessInformation for the device that is currently connected using this EventHandlerForDevice
        /// object.
        /// </summary>
        public DeviceAccessInformation DeviceAccessInformation
        {
            get
            {
                return deviceAccessInformation;
            }
        }

        /// <summary>
        /// DeviceSelector AQS used to find this device
        /// </summary>
        public String DeviceSelector
        {
            get
            {
                return deviceSelector;
            }
        }

        /// <summary>
        /// True if EventHandlerForDevice will attempt to reconnect to the device once it is plugged into the computer again
        /// </summary>
        public Boolean IsEnabledAutoReconnect
        {
            get
            {
                return isEnabledAutoReconnect;
            }
            set
            {
                isEnabledAutoReconnect = value;
            }
        }

        /// <summary>
        /// This method opens the device using the WinRT Usb API. After the device is opened, save the device
        /// so that it can be used across scenarios.
        ///
        /// It is important that the FromIdAsync call is made on the UI thread because the consent prompt can only be displayed
        /// on the UI thread.
        /// 
        /// This method is used to reopen the device after the device reconnects to the computer and when the app resumes.
        /// </summary>
        /// <param name="deviceInfo">Device information of the device to be opened</param>
        /// <param name="deviceSelector">The AQS used to find this device</param>
        /// <returns>True if the device was successfully opened, false if the device could not be opened for well known reasons.
        /// An exception may be thrown if the device could not be opened for extraordinary reasons.</returns>
        public async Task<Boolean> OpenDeviceAsync(DeviceInformation deviceInfo, String deviceSelector)
        {
            device = await UsbDevice.FromIdAsync(deviceInfo.Id);

            Boolean successfullyOpenedDevice = false;
            String notificationMessage = null;

            // Device could have been blocked by user or the device has already been opened by another app.
            if (device != null)
            {
                successfullyOpenedDevice = true;

                deviceInformation = deviceInfo;
                this.deviceSelector = deviceSelector;
                
                notificationMessage = "Device " + deviceInformation.Id + " opened";

                // Notify registered callback handle that the device has been opened
                if (deviceConnectedCallback != null)
                {
                    deviceConnectedCallback(this, deviceInformation);
                }

                // Background tasks are not part of the app, so app events will not have an affect on the device
                if (!isBackgroundTask && (appSuspendEventHandler == null || appResumeEventHandler == null))
                {
                    RegisterForAppEvents();
                }

                // User can block the device after it has been opened in the Settings charm. We can detect this by registering for the 
                // DeviceAccessInformation.AccessChanged event
                if (deviceAccessEventHandler == null)
                {
                    RegisterForDeviceAccessStatusChange();
                }

                // Create and register device watcher events for the device to be opened unless we're reopening the device
                if (deviceWatcher == null)
                {
                    deviceWatcher = DeviceInformation.CreateWatcher(deviceSelector);

                    RegisterForDeviceWatcherEvents();
                }

                if (!watcherStarted)
                {
                    // Start the device watcher after we made sure that the device is opened.
                    StartDeviceWatcher();
                }
            }
            else
            {
                successfullyOpenedDevice = false;

                var deviceAccessStatus = DeviceAccessInformation.CreateFromId(deviceInfo.Id).CurrentStatus;

                switch (deviceAccessStatus)
                {
                    case DeviceAccessStatus.DeniedByUser:
                        notificationMessage = "Access to the device was blocked by the user : " + deviceInfo.Id;
                        break;

                    case DeviceAccessStatus.DeniedBySystem:
                        // This status is most likely caused by app permissions (did not declare the device in the app's package.appxmanifest)
                        // This status does not cover the case where the device is already opened by another app.
                        notificationMessage = "Access to the device was blocked by the system : " + deviceInfo.Id;
                        break;

                    default:
                        // Most likely the device is opened by another app, but cannot be sure
                        notificationMessage = "Unknown error, possibly opened by another app : " + deviceInfo.Id;
                        break;
                }
            }

            return successfullyOpenedDevice;
        }

        /// <summary>
        /// Closes the device, stops the device watcher, stops listening for app events, and resets object state to before a device
        /// was ever connected.
        /// </summary>
        public void CloseDevice()
        {
            if (IsDeviceConnected)
            {
                CloseCurrentlyConnectedDevice();
            }

            if (deviceWatcher != null)
            {
                if (watcherStarted)
                {
                    StopDeviceWatcher();

                    UnregisterFromDeviceWatcherEvents();
                }

                deviceWatcher = null;
            }

            if (deviceAccessInformation != null)
            {
                UnregisterFromDeviceAccessStatusChange();

                deviceAccessInformation = null;
            }

            if (appSuspendEventHandler != null || appResumeEventHandler != null)
            {
                UnregisterFromAppEvents();
            }

            deviceInformation = null;
            deviceSelector = null;

            deviceConnectedCallback = null;
            deviceCloseCallback = null;
            appSuspendCallback = null;

            isEnabledAutoReconnect = true;
        }

        /// <summary>
        /// If this event handler will be running in a background task, app events will not be registered for because they are of
        /// no use to the background task.
        /// </summary>
        /// <param name="isBackgroundTask">Whether or not the event handler will be running as a background task</param>
        private EventHandlerForDevice(Boolean isBackgroundTask)
        {
            watcherStarted = false;
            watcherSuspended = false;
            isEnabledAutoReconnect = true;
            this.isBackgroundTask = isBackgroundTask;
        }

        /// <summary>
        /// This method demonstrates how to close the device properly using the WinRT Usb API.
        ///
        /// When the UsbDevice is closing, it will cancel all IO operations that are still pending (not complete).
        /// The close will not wait for any IO completion callbacks to be called, so the close call may complete before any of
        /// the IO completion callbacks are called.
        /// The pending IO operations will still call their respective completion callbacks with either a task 
        /// cancelled error or the operation completed.
        /// </summary>
        private async void CloseCurrentlyConnectedDevice()
        {
            if (device != null)
            {
                // Notify callback that we're about to close the device
                if (deviceCloseCallback != null)
                {
                    deviceCloseCallback(this, deviceInformation);
                }

                // This closes the handle to the device
                device.Dispose();

                device = null;

                // Save the deviceInformation.Id in case deviceInformation is set to null when closing the
                // device
                String deviceId = deviceInformation.Id;

                await rootPage.Dispatcher.RunAsync(
                    CoreDispatcherPriority.Normal,
                    new DispatchedHandler(() =>
                    {

                    }));
            }
        }

        /// <summary>
        /// Register for app suspension/resume events. See the comments
        /// for the event handlers for more information on what is being done to the device.
        ///
        /// We will also register for when the app exists so that we may close the device handle.
        /// </summary>
        private void RegisterForAppEvents()
        {
            appSuspendEventHandler = new SuspendingEventHandler(EventHandlerForDevice.Current.OnAppSuspension);
            appResumeEventHandler = new EventHandler<Object>(EventHandlerForDevice.Current.OnAppResume);

            // This event is raised when the app is exited and when the app is suspended
            App.Current.Suspending += appSuspendEventHandler;

            App.Current.Resuming += appResumeEventHandler;
        }

        private void UnregisterFromAppEvents()
        {
            // This event is raised when the app is exited and when the app is suspended
            App.Current.Suspending -= appSuspendEventHandler;
            appSuspendEventHandler = null;

            App.Current.Resuming -= appResumeEventHandler;
            appResumeEventHandler = null;
        }

        /// <summary>
        /// Register for Added and Removed events.
        /// Note that, when disconnecting the device, the device may be closed by the system before the OnDeviceRemoved callback is invoked.
        /// </summary>
        private void RegisterForDeviceWatcherEvents()
        {
            deviceAddedEventHandler = new TypedEventHandler<DeviceWatcher, DeviceInformation>(this.OnDeviceAdded);

            deviceRemovedEventHandler = new TypedEventHandler<DeviceWatcher, DeviceInformationUpdate>(this.OnDeviceRemoved);

            deviceWatcher.Added += deviceAddedEventHandler;

            deviceWatcher.Removed += deviceRemovedEventHandler;
        }

        private void UnregisterFromDeviceWatcherEvents()
        {
            deviceWatcher.Added -= deviceAddedEventHandler;
            deviceAddedEventHandler = null;

            deviceWatcher.Removed -= deviceRemovedEventHandler;
            deviceRemovedEventHandler = null;
        }

        /// <summary>
        /// Listen for any changed in device access permission. The user can block access to the device while the device is in use.
        /// If the user blocks access to the device while the device is opened, the device's handle will be closed automatically by
        /// the system; it is still a good idea to close the device explicitly so that resources are cleaned up.
        /// 
        /// Note that by the time the AccessChanged event is raised, the device handle may already be closed by the system.
        /// </summary>
        private void RegisterForDeviceAccessStatusChange()
        {
            deviceAccessInformation = DeviceAccessInformation.CreateFromId(deviceInformation.Id);

            deviceAccessEventHandler = new TypedEventHandler<DeviceAccessInformation, DeviceAccessChangedEventArgs>(this.OnDeviceAccessChanged);
            deviceAccessInformation.AccessChanged += deviceAccessEventHandler;
        }

        private void UnregisterFromDeviceAccessStatusChange()
        {
            deviceAccessInformation.AccessChanged -= deviceAccessEventHandler;

            deviceAccessEventHandler = null;
        }

        private void StartDeviceWatcher()
        {
            watcherStarted = true;

            if ((deviceWatcher.Status != DeviceWatcherStatus.Started)
                && (deviceWatcher.Status != DeviceWatcherStatus.EnumerationCompleted))
            {
                deviceWatcher.Start();
            }
        }

        private void StopDeviceWatcher()
        {
            if ((deviceWatcher.Status == DeviceWatcherStatus.Started)
                || (deviceWatcher.Status == DeviceWatcherStatus.EnumerationCompleted))
            {
                deviceWatcher.Stop();
            }

            watcherStarted = false;
        }

        /// <summary>
        /// If a UsbDevice object has been instantiated (a handle to the device is opened), we must close it before the app 
        /// goes into suspension because the API automatically closes it for us if we don't. When resuming, the API will
        /// not reopen the device automatically, so we need to explicitly open the device in the app (Scenario1_DeviceConnect).
        ///
        /// Since we have to reopen the device ourselves when the app resumes, it is good practice to explicitly call the close
        /// in the app as well (For every open there is a close).
        /// 
        /// We must stop the DeviceWatcher because it will continue to raise events even if
        /// the app is in suspension, which is not desired (drains battery). We resume the device watcher once the app resumes again.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void OnAppSuspension(Object sender, SuspendingEventArgs args)
        {
            if (watcherStarted)
            {
                watcherSuspended = true;
                StopDeviceWatcher();
            }
            else
            {
                watcherSuspended = false;
            }

            // Forward suspend event to registered callback function
            if (appSuspendCallback != null)
            {
                appSuspendCallback(sender, args);
            }

            CloseCurrentlyConnectedDevice();
        }

        /// <summary>
        /// When resume into the application, we should reopen a handle to the Usb device again. This will automatically
        /// happen when we start the device watcher again; the device will be re-enumerated and we will attempt to reopen it
        /// if IsEnabledAutoReconnect property is enabled.
        /// 
        /// See OnAppSuspension for why we are starting the device watcher again
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="arg"></param>
        private void OnAppResume(Object sender, Object args)
        {
            if (watcherSuspended)
            {
                watcherSuspended = false;
                StartDeviceWatcher();
            }
        }

        /// <summary>
        /// Close the device that is opened so that all pending operations are canceled properly.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="deviceInformationUpdate"></param>
        private void OnDeviceRemoved(DeviceWatcher sender, DeviceInformationUpdate deviceInformationUpdate)
        {
            if (IsDeviceConnected && (deviceInformationUpdate.Id == deviceInformation.Id))
            {
                // The main reasons to close the device explicitly is to clean up resources, to properly handle errors,
                // and stop talking to the disconnected device.
                CloseCurrentlyConnectedDevice();
            }
        }

        /// <summary>
        /// Open the device that the user wanted to open if it hasn't been opened yet and auto reconnect is enabled.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="deviceInfo"></param>
        private async void OnDeviceAdded(DeviceWatcher sender, DeviceInformation deviceInfo)
        {
            if ((deviceInformation != null) && (deviceInfo.Id == deviceInformation.Id) && !IsDeviceConnected && isEnabledAutoReconnect)
            {
                await rootPage.Dispatcher.RunAsync(
                    CoreDispatcherPriority.Normal,
                    new DispatchedHandler(async () =>
                    {
                        await OpenDeviceAsync(deviceInformation, deviceSelector);

                        // Any app specific device intialization should be done here because we don't know the state of the device when it is re-enumerated.
                    }));
            }
        }

        /// <summary>
        /// Close the device if the device access was denied by anyone (system or the user) and reopen it if permissions are allowed again
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private async void OnDeviceAccessChanged(DeviceAccessInformation sender, DeviceAccessChangedEventArgs eventArgs)
        {
            if ((eventArgs.Status == DeviceAccessStatus.DeniedBySystem)
                || (eventArgs.Status == DeviceAccessStatus.DeniedByUser))
            {
                CloseCurrentlyConnectedDevice();
            }
            else if ((eventArgs.Status == DeviceAccessStatus.Allowed) && (deviceInformation != null) && isEnabledAutoReconnect)
            {
                await rootPage.Dispatcher.RunAsync(
                    CoreDispatcherPriority.Normal,
                    new DispatchedHandler(async () =>
                    {
                        await OpenDeviceAsync(deviceInformation, deviceSelector);

                        // Any app specific device intialization should be done here because we don't know the state of the device when it is re-enumerated.
                    }));
            }
        }
    }
}
