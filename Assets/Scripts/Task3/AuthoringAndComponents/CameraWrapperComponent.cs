﻿using Unity.Entities;
using UnityEngine;

namespace Task3.AuthoringAndComponents
{
    public class CameraWrapperComponent : IComponentData
    {
        public Camera Camera;
    }
}