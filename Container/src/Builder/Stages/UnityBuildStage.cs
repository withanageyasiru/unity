﻿

namespace Unity.Builder
{
    /// <summary>
    /// The build stages we use in the Unity container
    /// strategy pipeline.
    /// </summary>
    public enum UnityBuildStage
    {
        /// <summary>
        /// First stage. By default, nothing happens here.
        /// </summary>
        Setup,

        /// <summary>
        /// Stage where Array or IEnumerable is resolved
        /// </summary>
        Enumerable,

        /// <summary>
        /// Third stage. lifetime managers are checked here,
        /// and if they're available the rest of the pipeline is skipped.
        /// </summary>
        Lifetime,

        /// <summary>
        /// Second stage. Type mapping occurs here.
        /// </summary>
        TypeMapping,

        /// <summary>
        /// Fourth stage. Reflection over constructors, properties, etc. is
        /// performed here.
        /// </summary>
        PreCreation,

        /// <summary>
        /// Fifth stage. Instance creation happens here.
        /// </summary>
        Creation,

        /// <summary>
        /// Sixth stage. Property sets and method injection happens here.
        /// </summary>
        Initialization,

        /// <summary>
        /// Seventh and final stage. By default, nothing happens here.
        /// </summary>
        PostInitialization
    }
}
