// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 22/03/2020
//
//  Copyright � 2017-2020 "Touky" <touky@prateek.top>
//
//  Prateek is free software. It comes without any warranty, to
//  the extent permitted by applicable law. You can redistribute it
//  and/or modify it under the terms of the Do What the Fuck You Want
//  to Public License, Version 2, as published by the WTFPL Task Force.
//  See http://www.wtfpl.net/ for more details.
//
// -END_PRATEEK_COPYRIGHT-

// -BEGIN_PRATEEK_CSHARP_IFDEF-
//-----------------------------------------------------------------------------
#region Prateek Ifdefs

//Auto activate some of the prateek defines
#if UNITY_EDITOR

//Auto activate debug
#if !PRATEEK_DEBUG
#define PRATEEK_DEBUG
#endif //!PRATEEK_DEBUG

#endif //UNITY_EDITOR && !PRATEEK_DEBUG

#endregion Prateek Ifdefs
// -END_PRATEEK_CSHARP_IFDEF-

//-----------------------------------------------------------------------------
namespace Prateek.Runtime.Core.Helpers
{
    using System;
    using System.Reflection;

    ///-------------------------------------------------------------------------
    public static class Types
    {
        public static Type GetType(string type_name)
        {
            // Try Type.GetType() first. This will work with types defined
            // by the Mono runtime, in the same assembly as the caller, etc.
            var type = Type.GetType(type_name);

            // If it worked, then we're done here
            if (type != null)
                return type;

            // If the TypeName is a full name, then we can try loading the defining assembly directly
            if (type_name.Contains("."))
            {

                // Get the name of the assembly (Assumption is that we are using 
                // fully-qualified type names)
                var assembly_name = type_name.Substring(0, type_name.IndexOf('.'));

                // Attempt to load the indicated Assembly
                var assembly = Assembly.Load(assembly_name);
                if (assembly == null)
                    return null;

                // Ask that assembly to return the proper Type
                type = assembly.GetType(type_name);
                if (type != null)
                    return type;

            }

            // If we still haven't found the proper type, we can enumerate all of the 
            // loaded assemblies and see if any of them define the type
            var current_assembly = Assembly.GetExecutingAssembly();
            var referenced_assemblies = current_assembly.GetReferencedAssemblies();
            for (int i = 0; i < referenced_assemblies.Length; i++)
            {
                var assembly_name = referenced_assemblies[i];

                // Load the referenced assembly
                var assembly = Assembly.Load(assembly_name);
                if (assembly != null)
                {
                    // See if that assembly defines the named type
                    type = assembly.GetType(type_name);
                    if (type != null)
                        return type;
                }
            }

            // The type just couldn't be found...
            return null;
        }
    }
}