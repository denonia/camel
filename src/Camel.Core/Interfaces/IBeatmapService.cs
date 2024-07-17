﻿using Camel.Core.Entities;

namespace Camel.Core.Interfaces;

public interface IBeatmapService
{
    Task<Beatmap?> FindBeatmapAsync(string md5);
}