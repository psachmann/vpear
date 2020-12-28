# Master Thesis: Visualization of Pressure Effects using an Augmented Reality Android App for Decubitus Prophylaxis

## Status

| Branches | Master | Develop |
| - | - | - |
| Build | ![Build Status][1] | ![Build Status][2] |

[1]: https://dev.azure.com/psachmann/vpear/_apis/build/status/psachmann.vpear?branchName=master
[2]: https://dev.azure.com/psachmann/vpear/_apis/build/status/psachmann.vpear?branchName=develop

---
## Overview

TODO: write short introduction

---
## Progress

### Server

| Feature | Controller | Service | Validators |
| :- | :-: | :-: | :-: |
| Device | `InProgress` | `InProgress` | `InProgress` |
| Filter | Done | Done | Done |
| Firmware | `InProgress` | `InProgress` | `InProgress` |
| Home | Done | - | - |
| Power | Done | Done | - |
| Sensor | `InProgress` | Done | - |
| User | `InProgress` | `InProgress` | `InProgress` |
| Wifi | Done | Done | Done |
| Reading Device | - | `NotStarted` | - |
| DeviceClient | - | `InProgress`  | - |
| VPEARClient | - | `NotStarted`  | - |

### Security

| Feature | Status | Description |
| :- | :-: | :- |

### Client

| Feature | Status | Description |
| :- | :-: | :- |

---
## Wep Api

| Route | Method | Function |
| - | - | - |
| /api/v1 | GET | looks if the server is alive |
| /api/v1/device?id={id} | GET | gets the device details with id |
| /api/v1/device?id={id} | DELETE | deletes the device with id |
| /api/v1/device | POST | creates a new device |
| /api/v1/device?id={id} | PUT | sets the device details |
| /api/v1/device/sensors?id={id} | GET | gets the device sensors |
| /api/v1/device/frames?id={id},start={start},stop={stop} | GET | gets the device frames |
| /api/v1/device/filters?id={id} | GET | gets the device filters |
| /api/v1/device/filters?id={id} | PUT | sets the device filters |
| /api/v1/device/power?id={id} | GET | gets the device power details |
| /api/v1/device/wifi?id={id} | GET | gets the device wifi details |
| /api/v1/device/wifi?id={id} | PUT | sets the device wifi details |
| /api/v1/device/firmware?id={id} | GET | gets the device firmware details |
| /api/v1/device/firmware?id={id} | PUT | sets the device firmware details |
| /api/v1/device/sse |||
