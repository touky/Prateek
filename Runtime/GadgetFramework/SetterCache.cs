namespace Prateek.Runtime.GadgetFramework
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    internal class SetterCache
    {
        public Dictionary<Type, PropertyInfo> pouchSetters = new Dictionary<Type, PropertyInfo>(50);
        public Dictionary<Type, List<PropertyInfo>> ownerSetters = new Dictionary<Type, List<PropertyInfo>>(50);
        public Dictionary<Type, PropertyInfo> gadgetSetters = new Dictionary<Type, PropertyInfo>(50);
    }
}