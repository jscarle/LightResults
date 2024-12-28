---
_layout: landing
---

# LightResults - Operation Result Patterns for .NET

<a class="github-button" href="https://github.com/jscarle/LightResults" data-color-scheme="no-preference: light; light: light; dark: dark;" data-icon="octicon-star" data-size="large" aria-label="Star LightResults on GitHub">Star</a>
<a class="github-button" href="https://github.com/jscarle/LightResults/issues" data-color-scheme="no-preference: light; light: light; dark: dark;" data-icon="octicon-issue-opened" data-size="large" aria-label="Open issue for LightResults on GitHub">Issue</a>
<a class="github-button" href="https://github.com/sponsors/jscarle" data-color-scheme="no-preference: light; light: light; dark: dark;" data-icon="octicon-heart" data-size="large" aria-label="Sponsor @jscarle on GitHub">Sponsor</a>

LightResults is an extremely light and modern .NET library that provides a simple and flexible
implementation of the Result Pattern. The Result Pattern is a way of representing the outcome
of an operation, whether it's successful or has encountered an error, in a more explicit and
structured manner. This project is heavily inspired by [Michael Altmann](https://github.com/altmann)'s
excellent work with [FluentResults](https://github.com/altmann/FluentResults).

[![main](https://img.shields.io/github/actions/workflow/status/jscarle/LightResults/main.yml?logo=github)](https://github.com/jscarle/LightResults)
[![nuget](https://img.shields.io/nuget/v/LightResults)](https://www.nuget.org/packages/LightResults)
[![downloads](https://img.shields.io/nuget/dt/LightResults)](https://www.nuget.org/packages/LightResults)

## References

This library targets .NET Standard 2.0, .NET 6.0, .NET 7.0, .NET 8.0, and .NET 9.0.

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
- 🚀 Performant — Heavily optimized and [benchmarked](https://jscarle.github.io/LightResults/docs/performance.html) to aim for the highest possible performance.

## Extensions

Several [extensions are available](https://github.com/jscarle/LightResults.Extensions) to simplify implementation that use LightResults.
