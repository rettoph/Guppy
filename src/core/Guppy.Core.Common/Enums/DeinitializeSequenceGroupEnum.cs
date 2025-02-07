namespace Guppy.Core.Common.Enums
{
    /// <summary>
    /// Note: These values are opposite those of <see cref="InitializeSequenceGroupEnum"/>
    /// This is because we want to deinitialize in reverse order of initialization:
    /// 
    /// This means Setup initialize methods will be called first but Setup deinitialize methods will
    /// be called last
    /// </summary>
    public enum DeinitializeSequenceGroupEnum
    {
        Cleanup,
        PostInitialize,
        Initialize,
        PreInitialize,
        Setup
    }
}