namespace Prateek.Runtime.KeynameFramework.Enums
{
    public enum KeynameMatchType
    {
        /// <summary>
        ///     Both ID have the same name and number of tags
        ///     Left ID: TagA_TagB_TagC_MyName
        ///     Right ID: TagA_TagB_TagC_MyName
        /// </summary>
        Equal,

        /// <summary>
        ///     Left ID is of Filter Type, ALL the left tags match SOME of the right tags
        ///     Left ID: TagA_TagC
        ///     Right ID: TagA_TagB_TagC_MyName
        /// </summary>
        MatchFull,

        /// <summary>
        ///     Left ID is of Filter Type and matches SOME of its tags
        ///     Left ID: TagA_TagC_TagD
        ///     Right ID: TagA_TagB_TagC_MyName
        /// </summary>
        MatchPartial,

        /// <summary>
        ///     Left ID is of Filter Type and matches NONE of its tags
        ///     Left ID: TagD_TagE_TagF
        ///     Right ID: TagA_TagB_TagC_MyName
        /// </summary>
        MatchFail
    }
}
