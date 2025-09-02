# Webamoki.Utils

A comprehensive utility library for .NET C# applications, providing essential tools for cryptography, file handling, logging, email functionality, and more.

[![NuGet](https://img.shields.io/nuget/v/Webamoki.Utils.svg)](https://www.nuget.org/packages/Webamoki.Utils/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

## Installation

Install the package via NuGet Package Manager:

```bash
dotnet add package Webamoki.Utils
```

Or via Package Manager Console:

```powershell
Install-Package Webamoki.Utils
```

## Features

### 🔐 Cryptography
- **Secure Hashing**: One-way hashing with BCrypt
- **Two-way Encryption**: AES-256-CBC encryption/decryption
- **Random Generation**: Cryptographically secure random strings and numbers
- **Base Conversion**: Convert integers to custom base representations

### 📁 File Handling
- **File Operations**: Read/write text and binary files
- **Embedded Resources**: Access embedded resources from assemblies
- **Directory Management**: Automatic directory creation

### 📧 Email System
- **HTML Email Builder**: Rich email template system with styling
- **SMTP Integration**: AWS SES integration for reliable email delivery
- **Attachments**: Support for file and inline image attachments
- **Unsubscribe Links**: Built-in unsubscribe functionality

### 🔍 Validation
- **Value Validation**: Comprehensive validation for various data types
- **CSS Validation**: Specialized validation for CSS values (pixels, rem, percentages)
- **Security Checks**: Built-in protection against malicious input

### 📝 Logging
- **Console Logging**: Colored console output with different log levels (Debug, Info, Warn, Error)
- **Flexible Control**: Enable/disable logging anytime without errors
- **Log Buffering**: Hold logs with labels for later retrieval and analysis
- **Thread-Safe**: Concurrent access support for multi-threaded applications

### 🔧 String Extensions
- **Enhanced String Operations**: Additional string manipulation methods

## Quick Start

### Cryptography

```csharp
using Webamoki.Utils;

// Hash a password
string hashedPassword = Cryptography.Hash("mypassword");

// Verify a password
bool isValid = Cryptography.Verify("mypassword", hashedPassword);

// Generate random string
string randomId = Cryptography.GenerateRandomString(10);

// Secure encryption
string encrypted = Cryptography.SecureEncode("sensitive data", "user-context");
string decrypted = Cryptography.SecureDecode(encrypted, "user-context");
```

### File Operations

```csharp
using Webamoki.Utils;

// Write to file
FileHandler.Write("data/config.txt", "configuration data");

// Read from file
string content = FileHandler.ReadText("data/config.txt");
byte[] binaryData = FileHandler.ReadBytes("data/image.png");

// Access embedded resources
var assembly = Assembly.GetExecutingAssembly();
Stream? resourceStream = EmbeddedResourceHandler.Read(assembly, "Templates.Email.html");
```

### Email System

```csharp
using Webamoki.Utils;

// Create email with rich HTML content
var mailBody = new MailBody("Welcome to Our Service");
mailBody.AddParagraph("Thank you for joining us!");
mailBody.AddButton("Get Started", "https://example.com/start", "#007bff");
mailBody.AddUnsubscribeLink("https://example.com/unsubscribe");

// Send email
var mailSender = new MailSender("Welcome Email");
mailSender.Message.To.Add("user@example.com");
mailSender.AddHTMLBody(mailBody.GetHTMLBody(
    companyName: "Your Company",
    contactPhone: "+1234567890",
    contactEmail: "support@example.com",
    colorValue: "#007bff",
    companyLogoUrl: "logo.png",
    baseUrl: "https://example.com"
));
mailSender.Send();
```

### Validation

```csharp
using Webamoki.Utils;

// Validate CSS values
bool isValidCss = ValueValidations.CheckGlobalCSS("100px", stringPixel: true);

// Check for banned characters
bool hasBannedChars = ValueValidations.HasBannedCharacters(userInput);

// Custom validation
bool isValid = ValueValidations.Check(
    value: "50%",
    stringPercentage: true,
    allowNegative: false
);
```

### Logging

```csharp
using Webamoki.Utils;

// Enable/disable logging (safe to call multiple times)
Logging.Enable();
Logging.Disable();
Logging.IsLoggingEnabled = true; // Can also set directly

// Log messages with different levels
Logging.WriteDebug("Debug information");
Logging.WriteInfo("Application started");
Logging.WriteWarn("Warning: Low disk space");
Logging.WriteError("Error occurred");

// Custom colors
Logging.WriteLog("Custom message", LoggingLevel.Info, ConsoleColor.Green);

// Buffer logs with labels for later retrieval
Logging.WriteDebug("User logged in", "user-actions");
Logging.WriteInfo("User updated profile", "user-actions");
Logging.WriteError("Database connection failed", "errors");

// Initialize a buffer (optional - buffers are created automatically)
Logging.Hold("system-events");

// Retrieve buffered logs
var userActionLogs = Logging.GetHeldLogs("user-actions");
var errorLogs = Logging.GetHeldLogs("errors");

// Get all buffer labels
var allLabels = Logging.GetBufferLabels();

// Clear specific or all buffered logs
Logging.ClearHeldLogs("user-actions");
Logging.ClearAllHeldLogs();
```

## Dependencies

- **BCrypt.Net-Next** (4.0.3): For secure password hashing
- **Microsoft.AspNetCore.Mvc.ViewFeatures** (2.2.0): For MVC integration
- **Microsoft.AspNetCore.Session** (2.2.0): For session management

## Requirements

- **.NET 9.0** or later

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## Support

For support and questions, please contact us through our [GitHub repository](https://github.com/Webamoki/Webamoki.Utils).

---

**Webamoki.Utils** - Simplifying .NET development with essential utilities.