// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 05/03/19
//
//  Copyright © 2017—2019 Benjamin "Touky" Huet <huet.benjamin@gmail.com>
//
//  Prateek is free software. It comes without any warranty, to
//  the extent permitted by applicable law. You can redistribute it
//  and/or modify it under the terms of the Do What the Fuck You Want
//  to Public License, Version 2, as published by the WTFPL Task Force.
//  See http://www.wtfpl.net/ for more details.
//
// -END_PRATEEK_COPYRIGHT-

// -BEGIN_PRATEEK_CSHARP_NAMESPACE-
//
#region C# Prateek Namespaces
#if UNITY_EDITOR && !PRATEEK_DEBUG
#define PRATEEK_DEBUG
#endif //UNITY_EDITOR && !PRATEEK_DEBUG

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine.Jobs;

#if UNITY_EDITOR
using UnityEditor;
#endif //UNITY_EDITOR

#if UNITY_PROFILING
using UnityEngine.Profiling;
#endif //UNITY_PROFILING

using Prateek;
using Prateek.Base;
using Prateek.Extensions;
using Prateek.Helpers;
using Prateek.Attributes;

#if PRATEEK_DEBUGS
using Prateek.Debug;
#endif //PRATEEK_DEBUG
#endregion C# Prateek Namespaces
//
// -END_PRATEEK_CSHARP_NAMESPACE-

//-----------------------------------------------------------------------------
namespace ScriptTemplating
{
    //-------------------------------------------------------------------------
    [InitializeOnLoad]
    class CSharpTemplate : ScriptTemplateReplacement
    {
        static CSharpTemplate()
        {
            NewData(".cs")
            .SetMatching(": MonoBehaviour", "Start()", "Update()")
            .SetContent(
"#PRATEEK_COPYRIGHT#",
"",
"#PRATEEK_CSHARP_NAMESPACE#",
"",
"//-----------------------------------------------------------------------------",
"#region File namespaces",
"#endregion File namespaces",
"",
"//-----------------------------------------------------------------------------",
"namespace Gameplay",
"{",
"    //-------------------------------------------------------------------------",
"    public partial class #SCRIPTNAME# : BaseBehaviour",
"    {",
"        //---------------------------------------------------------------------",
"        #region Declarations",
"        #endregion Declarations",
"",
"        //---------------------------------------------------------------------",
"        #region Settings",
"        #endregion Settings",
"",
"        //---------------------------------------------------------------------",
"        #region Fields",
"        #endregion Fields",
"",
"        //---------------------------------------------------------------------",
"        #region Properties",
"        #endregion Properties",
"",
"        //---------------------------------------------------------------------",
"        #region Unity Defaults",
"        private void Awake()",
"        {",
"        }",
"",
"        //---------------------------------------------------------------------",
"        private void OnDestroy()",
"        {",
"        }",
"",
"        //---------------------------------------------------------------------",
"        private void Start()",
"        {",
"        }",
"",
"        //---------------------------------------------------------------------",
"        private void Update()",
"        {",
"        }",
"        #endregion Unity Defaults",
"",
"        //---------------------------------------------------------------------",
"        #region Behaviour",
"        #endregion Behaviour",
"    }",
"}",
"")
            .Submit();

            NewKeyword(".cs")
                .SetKeyword("PRATEEK_CSHARP_NAMESPACE")
                .SetContent(
"// -BEGIN_PRATEEK_CSHARP_NAMESPACE-",
"//",
"#region C# Prateek Namespaces",
"#if UNITY_EDITOR && !PRATEEK_DEBUG",
"#define PRATEEK_DEBUG",
"#endif //UNITY_EDITOR && !PRATEEK_DEBUG",
"",
"using System;",
"using System.Collections;",
"using System.Collections.Generic;",
"using UnityEngine;",
"using UnityEngine.Serialization;",
"using Unity.Collections;",
"using Unity.Jobs;",
"using UnityEngine.Jobs;",
"",
"#if UNITY_EDITOR",
"using UnityEditor;",
"#endif //UNITY_EDITOR",
"",
"#if UNITY_PROFILING",
"using UnityEngine.Profiling;",
"#endif //UNITY_PROFILING",
"",
"using Prateek;",
"using Prateek.Base;",
"using Prateek.Extensions;",
"using Prateek.Helpers;",
"using Prateek.Attributes;",
"",
"#if PRATEEK_DEBUGS",
"using Prateek.Debug;",
"#endif //PRATEEK_DEBUG",
"#endregion C# Prateek Namespaces",
"//",
"// -END_PRATEEK_CSHARP_NAMESPACE-")
                .Submit();
        }
    }
}
