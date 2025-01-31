# QuestionsWebApp in .Net

QuestionsWebApp is a .Net web application that allows users to test their knowledge of world geography by identifying countries and their capitals.

## Requirements

- dotnet-sdk-8.0

## Getting Started

1. Clone the repository:
   ```bash
   git clone https://github.com/Eclipse91/QuestionsWebAppDotNet.git
   ```

2. Navigate to the project directory:
   ```bash
   cd QuestionsWebAppDotNet
   ```

3. Restore Dependencies
Run this command inside your project folder:
```sh
dotnet restore
```
This will download missing NuGet packages.

4. Build the Project
To compile your project, run:
```sh
dotnet build
```

5. Run the Project
To execute the program:
```sh
dotnet run
```

## Install dotnet-sdk-8.0
### **1. Install .NET SDK on Linux**
First, you need to install the .NET SDK for Linux. Open a terminal and install it based on your distribution:

- **Ubuntu/Debian-based:**
  ```sh
  wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
  sudo dpkg -i packages-microsoft-prod.deb
  sudo apt-get update
  sudo apt-get install -y dotnet-sdk-8.0
  ```
  *(Replace `8.0` with your project's .NET version if different.)*

- **Fedora:**
  ```sh
  sudo dnf install dotnet-sdk-8.0
  ```

- **Arch Linux:**
  ```sh
  sudo pacman -S dotnet-sdk
  ```

To verify installation, run:
```sh
dotnet --version
```

## License

This project is licensed under the GNU GENERAL PUBLIC LICENSE - see the [LICENSE](LICENSE) file for details.

## Notes

Feel free to contribute or report issues!
This README provides a clearer structure, concise information, and instructions for setting up and running the PasswordGeneratorApp. Adjust the content as needed for your project.