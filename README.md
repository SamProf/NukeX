# NukeX

NukeX is a faster alternative to the standard `Nuke.Build` launcher. It intelligently re-builds your project only when necessary, based on changes in your files. NukeX is designed to be installed as a dotnet tool and can run Targets with ease. Future versions will support most of the original Nuke commands, as it will hand over control to Nuke when needed.

## Table of Contents

- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Usage](#usage)
- [Examples](#examples)
- [Contributing](#contributing)
- [License](#license)

## Prerequisites

Before installing NukeX, ensure that you have the following software installed on your machine:

- .NET SDK (version 6.0 or higher)

## Installation

To install NukeX as a dotnet tool, open your terminal and run the following command:

```bash
dotnet tool install --global NukeX
```

This will install the NukeX tool globally on your machine.

## Usage

To use NukeX, navigate to the directory of your project and run the following command:

```bash
NukeX [TARGET]
```

Replace `[TARGET]` with the target you want to execute. If you don't specify a target, NukeX will run the default target specified in your Nuke configuration.

## Examples

Here are some examples of how to use NukeX:

1. Running the default target:

```bash
NukeX
```

2. Running a specific target, for example, `Compile`:

```bash
NukeX Compile
```

3. Running multiple targets, for example, `Clean` and `Compile`:

```bash
NukeX Clean Compile
```

## Contributing

We welcome contributions to the NukeX project. If you want to contribute, please follow these steps:

1. Fork the repository.
2. Create a new branch for your changes.
3. Commit your changes to the new branch.
4. Create a pull request with a description of your changes.

We will review your pull request and provide feedback.

## License

NukeX is released under the [MIT License](LICENSE).
