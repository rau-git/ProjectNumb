using FishNet.Documenting;
using System;
using UnityEngine;

namespace FishNet.Component.Prediction
{
    [AddComponentMenu("")]
    [APIExclude]
    [Obsolete("PredictedRigidbody2D is obsolete. Please remove this component and use PredictedObject on your gameObject's root.")]  
    //Remove on 2023/01/01
    public class PredictedRigidbody2D : PredictedRigidbodyBase { }


}