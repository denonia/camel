﻿namespace Camel.Bancho.Enums;

[Flags]
public enum Privileges : int
{
    Player = 1 << 0,
    Moderator = 1 << 1,
    Supporter = 1 << 2,
    Owner = 1 << 3,
    Developer = 1 << 4,
    Tournament = 1 << 5
}