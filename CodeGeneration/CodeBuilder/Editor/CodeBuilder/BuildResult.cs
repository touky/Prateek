namespace Assets.Prateek.CodeGenerator.Code.CodeBuilder {
    using System;

    public struct BuildResult
    {
        ///-----------------------------------------------------------------
        [Flags]
        public enum ValueType
        {
            Success = 1 << 0,
            Ignored = 1 << 1,

            LoadingFailed = 1 << 2,
            NoMatchingTemplate = 1 << 3,
            WritingFailedDirDoesntExist = 1 << 4,
            PrateekScriptSourceStartTagInvalid = 1 << 5,
            PrateekScriptArgNotFound = 1 << 6,
            PrateekScriptDataNotFound = 1 << 7,
            PrateekScriptDataNotTreated = 1 << 8,
            PrateekScriptInvalidKeyword = 1 << 9,
            PrateekScriptInsufficientNames = 1 << 10,
            PrateekScriptSourceDataTagInvalid = 1 << 11,
            PrateekScriptInsufficientFuncResults = 1 << 12,
            PrateekScriptKeywordCannotStartWithNumeric = 1 << 13,
            PrateekScriptWrongKeywordChar = 1 << 14,


            MAX = 32
        }

        ///-----------------------------------------------------------------
        private ValueType value;
        private string text;

        ///-----------------------------------------------------------------
        public ValueType Value { get { return value; } }
        public static implicit operator bool(BuildResult result)
        {
            return (result.value & ValueType.Success) != 0;
        }

        ///-----------------------------------------------------------------
        public static implicit operator BuildResult(ValueType value)
        {
            return new BuildResult() { value = value, text = String.Empty };
        }

        ///-----------------------------------------------------------------
        public static BuildResult operator +(BuildResult other, string text)
        {
            return new BuildResult() { value = other.value, text = other.text + (other.text != String.Empty ? ", " : "") + text };
        }

        ///-----------------------------------------------------------------
        public static BuildResult operator +(BuildResult one, BuildResult other)
        {
            return new BuildResult() { value = one.value | other.value, text = one.text + (one.text != String.Empty ? ", " : "") + other.text };
        }

        ///-----------------------------------------------------------------
        public bool Is(ValueType value)
        {
            return (this.value & value) != 0;
        }

        ///-----------------------------------------------------------------
        public void Log()
        {
            var log = String.Format("Error with: {0}\n", text);
            for (int i = 0; i < (int)ValueType.MAX; i++)
            {
                var testValue = (ValueType)(1 << i);
                if (!Is(testValue))
                    continue;
                log += String.Format(" - {0}\n", testValue.ToString());
            }
            UnityEngine.Debug.LogError(log);
        }
    }
}