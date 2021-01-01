# Master Thesis: Visualization of Pressure Effects using an Augmented Reality Android App for Decubitus Prophylaxis

## Status

| Branches | Master | Develop |
| - | - | - |
| Build | ![Build Status][1] | ![Build Status][2] |

[1]: https://dev.azure.com/psachmann/vpear/_apis/build/status/psachmann.vpear?branchName=master
[2]: https://dev.azure.com/psachmann/vpear/_apis/build/status/psachmann.vpear?branchName=develop

## Introduction

TODO: write short introduction

## Documentation

TODO: reference to the documentation

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
