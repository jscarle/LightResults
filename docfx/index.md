---
_layout: landing
---

# LightResults - Operation Result Patterns for .NET

LightResults is an extremely light and modern .NET library that provides a simple and flexible
implementation of the Result Pattern. The Result Pattern is a way of representing the outcome
of an operation, whether it's successful or has encountered an error, in a more explicit and
structured manner. This project is heavily inspired by [Michael Altmann](https://github.com/altmann)'s
excellent work with [FluentResults](https://github.com/altmann/FluentResults).

[![nuget](https://img.shields.io/nuget/v/LightResults)](https://www.nuget.org/packages/LightResults)
[![main](https://github.com/jscarle/LightResults/actions/workflows/main.yml/badge.svg)](https://github.com/jscarle/LightResults)

## References

This library targets .NET Standard 2.0, .NET 6.0, .NET 7.0, and .NET 8.0.

## Dependencies

This library has no dependencies.

## Advantages of this library

- 🪶 Lightweight — Only contains what's necessary to implement the Result Pattern.
- ⚙️ Extensible — Simple interfaces and base classes make it easy to adapt.
- 🧱 Immutable — Results and errors are immutable and cannot be changed after being created.
- 🧵 Thread-safe — The Error list and Metadata dictionary use Immutable classes for thread-safety.
- ✨ Modern — Built against the latest version of .NET using the most recent best practices.
- 🧪 Native — Written, compiled, and tested against the latest versions of .NET.
- ❤️ Compatible — Available for dozens of versions of .NET as a [.NET Standard 2.0](https://learn.microsoft.com/en-us/dotnet/standard/net-standard?tabs=net-standard-2-0) library.
- 🪚 Trimmable — Compatible with [ahead-of-time compilation](https://learn.microsoft.com/en-us/dotnet/core/deploying/native-aot/) (AOT) as of .NET 7.0.
- 🚀 Performant — Heavily optimized and benchmarked to aim for the highest possible performance.
