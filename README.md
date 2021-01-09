# Master Thesis: Visualization of Pressure Effects using an Augmented Reality Android App for Decubitus Prophylaxis

## Status

| Service | Status |
| - | - |
| GitHub | ![Build Status][build-status] |
| Coverage | ![Coverage Status][coverage-status] |

[build-status]: https://github.com/psachmann/vpear/workflows/VPEAR%20CI/badge.svg
[coverage-status]: https://raw.githubusercontent.com/psachmann/vpear-docs/main/report/badge_combined.svg

---
## Overview

TODO: write short introduction

---
## Progress

### Server

| Feature | Controller | Service | Validators |
| :- | :-: | :-: | :-: |
| Device | Done | Done | Done |
| Filter | Done | Done | Done |
| Firmware | Done | Done | Done |
| Home | Done | - | - |
| Power | Done | Done | - |
| Sensor | Done | Done | - |
| User | Done | Done | Done |
| Wifi | Done | Done | Done |
| Reading Device | - | `InProgress` | - |
| DeviceClient | - | Done  | - |
| VPEARClient | - | Done  | - |

### Security

| Feature | Status | Description |
| :- | :-: | :- |

### Client

| Feature | Status | Description |
| :- | :-: | :- |

## Device Status State Transitions

| Status | Archived | Not Reachable | Recording | Stopped |
| :- | :-: | :-: | :-: | :-: |
| Archived | Yes | No | No | No |
| Not Reachable | Yes | Yes | Yes | Yes |
| Recording | Yes | Yes | Yes | Yes |
| Stopped | Yes | Yes | Yes | Yes |

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

## License

This repository is licensed under the [MIT][license] license.

[license]: ./LICENSE.md
