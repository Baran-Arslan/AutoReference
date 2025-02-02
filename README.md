# AutoReference Dependency Injector for Unity

![License](https://img.shields.io/badge/License-MIT-blue.svg)
![Unity](https://img.shields.io/badge/Unity-2020.3%2B-green.svg)
![Version](https://img.shields.io/badge/Version-1.0.1-brightgreen.svg)
![AUTO Reference](https://github.com/user-attachments/assets/041292aa-8b58-4950-9c08-0dc2517b99b4)


## Overview

**AutoReference** is a powerful and intuitive dependency injection tool designed for Unity. It automatically assigns references to fields, reducing manual setup and streamlining your development workflow.

## Features

- **Automated Dependency Injection**: Automatically assigns references to fields, eliminating the need for manual setup.
- **Ease of Use**: Simple integration with your existing Unity projects.
- **Performance Optimized**: Zero impact on runtime performance!
- **Customizable**: Supports custom attributes and configurable settings for advanced use cases.

## Installation

1. Download the latest release from the [Releases](https://github.com/Baran-Arslan/AutoReference/releases/tag/v0.0.0) page.
2. Open your Unity project and navigate to `Assets` > `Import Package` > `Custom Package`.
3. Select the downloaded `.unitypackage` file and click `Open`.


## Usage
![AUTO Reference (1)](https://github.com/user-attachments/assets/5b748748-9c2b-483b-84cd-6a30dada67d8)


## Tutorial
[![Watch the video](https://img.youtube.com/vi/kcjdpvvFNY8/0.jpg)](https://www.youtube.com/watch?v=kcjdpvvFNY8&feature=youtu.be)


## Tests / Examples
**Tests and example files are included as an additional package within the asset.**

-Thanks to the tests, you can easily edit the asset, making sure that you do not break anything.

-You can find all the scripts used in the tutorial video in the examples package.

## Optimization
**Since it is an operation that only takes place on the editor, it will not make the slightest difference in performance to your game.**




By default, it searches for inject targets in all folders. When there are thousands of prefabs in your project, the inject process may slow down.
![image](https://github.com/user-attachments/assets/e777ebdd-68e1-4548-9714-24c64f2beeb5)



Therefore, if you drag the folders containing the Inject targets and services to the FolderPathManager, it will search only those folders. In this way, you can perform instant injection without searching for unnecessary assets.
![image](https://github.com/user-attachments/assets/40a646b4-aa95-4dc5-bd4a-67f1ca519c47)





## Further Assistance
If you need general troubleshooting or support, feel free to reach out via email at games.icare@gmail.com.
