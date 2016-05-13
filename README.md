<h1 align="center"><img width="200" src="media/logo.png" alt="Dice logo" style="clear: right;"><br/><br/></h1>

[![Build status](https://ci.appveyor.com/api/projects/status/7tms0gm6uivv2d84?svg=true)](https://ci.appveyor.com/project/virtyaluk/dice) [![NuGet](https://img.shields.io/nuget/v/Dice.svg?maxAge=7200)](https://www.nuget.org/packages/Dice/) [![Join the chat at https://gitter.im/modern-dev/dice](https://badges.gitter.im/modern-dev/dice.svg)](https://gitter.im/modern-dev/dice?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

# Dice

**Dice** - is a fake data generator for .NET.

```bash
PM> Install-Package Dice
```

## :clipboard: Usage

```csharp
var dice = new Dice();

// Generate random primitives
bool randBool = dice.NextBool();
char randChar = dice.NextChar();
string randStr = dice.NextString();

// Generate random sentence
var sentence = dice.NextSentence();

// Generate random city name
var city = dice.NextCity();

```

## :mortar_board: API Reference

### Embedded generators

TODO:

### Helpers

TODO:

### Seeding

TODO:

## :green_book: License

Heavily inspired by [https://github.com/chancejs/chancejs/](https://github.com/chancejs/chancejs/).
The icon was taken from [here](http://findicons.com/pack/2200/dices).

[Licensed under the MIT license.](https://github.com/virtyaluk/InTouch/blob/master/LICENSE)

Copyright (c) 2016 Bohdan Shtepan

---

> [modern-dev.com](http://modern-dev.com) &nbsp;&middot;&nbsp;
> GitHub [@virtyaluk](https://github.com/virtyaluk) &nbsp;&middot;&nbsp;
> Twitter [@virtyaluk](https://twitter.com/virtyaluk)
