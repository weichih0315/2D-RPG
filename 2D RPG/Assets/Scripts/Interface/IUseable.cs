﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUseable
{
    Sprite Icon { get; }

    bool Use();
}
