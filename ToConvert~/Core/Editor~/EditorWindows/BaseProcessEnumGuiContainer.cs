namespace Mayfair.Core.Editor.EditorWindows
{
    using System.Collections.Generic;
    using System.IO;
    using Mayfair.Core.Code.MathExt;
    using Mayfair.Core.Code.Utils.Helpers;
    using Mayfair.Core.Editor.GUI;
    using Mayfair.Core.Editor.Utils;
    using UnityEditor;
    using UnityEngine;

    public abstract class BaseProcessEnumGuiContainer : BaseEditorWindow
    {
        public const int NEXT_WORK = 1;
        public const int WORK_WAIT_FOR = 1;
        public const int NEXT_SLOT = 1;
        public const int NEXT_DRAW = 1;

        #region FieldSlot enum
        protected enum FieldSlot
        {
            ProcessFolder,
            ProcessFile,

            ArgumentSrcTag,
            ArgumentSrcDir,

            ArgumentDstTag,
            ArgumentDstDir,

            CustomSlot,
            CustomSlotMAX = CustomSlot + 100, 

            MAX
        }
        #endregion

        #region GUIDrawAction enum
        protected enum GUIDrawAction
        {
            DrawTags,
            DrawProcess,
            DrawAutoStart,
            DrawLogger,
            
            DrawCustom,
            DrawCustomMAX = DrawCustom + 5,

            MAX
        }
        #endregion

        #region GUIField enum
        protected enum GUIField
        {
            Label,
            Toggle,
            Value,
            Checkout,
            Existence,
            Clear,

            MAX
        }
        #endregion

        #region ProcessingStatusType enum
        protected enum ProcessingStatusType
        {
            None,
            PreExecute,
            Executing,
            PostExecute,
            Done
        }
        #endregion

        #region Fields
        private float[] tagDrawSizes = new float[(int) GUIField.MAX];
        private List<FieldSlot> fieldSlotDrawOrder = new List<FieldSlot>();
        #endregion

        #region Class Methods
        protected abstract void RefreshStatusOnFileExist(bool valueExists);
        protected abstract void SetFieldSlotTo(FieldSlot slot, string value);

        protected virtual string GetRelativePath(FieldSlot slot, FieldSlotInfos infos)
        {
            return string.Empty;
        }
        #endregion

        #region Nested type: FieldSlotInfos
        protected struct FieldSlotInfos
        {
            public string title;
            public FieldSlotTag status;
            public string defaultValue;

            public bool HasEither(FieldSlotTag status)
            {
                return this.status.HasEither(status);
            }
        }
        #endregion

        #region Main GUI Logic
        protected virtual void InitFieldSlotDrawOrder(List<FieldSlot> drawOrder)
        {
            if (drawOrder.Count != 0)
            {
                return;
            }

            drawOrder.AddRange(
                new[]
                {
                    FieldSlot.ProcessFolder,
                    FieldSlot.ProcessFile,
                    FieldSlot.ArgumentSrcTag,
                    FieldSlot.ArgumentSrcDir,
                    FieldSlot.ArgumentDstTag,
                    FieldSlot.ArgumentDstDir,
                    FieldSlot.CustomSlot + NEXT_SLOT * 1,
                    FieldSlot.CustomSlot + NEXT_SLOT * 2,
                    FieldSlot.CustomSlot + NEXT_SLOT * 3,
                    FieldSlot.CustomSlot + NEXT_SLOT * 4,
                    FieldSlot.CustomSlot + NEXT_SLOT * 5,
                    FieldSlot.CustomSlot + NEXT_SLOT * 6,
                    FieldSlot.CustomSlot + NEXT_SLOT * 7,
                    FieldSlot.CustomSlot + NEXT_SLOT * 8,
                    FieldSlot.CustomSlot + NEXT_SLOT * 9
                });
        }

        protected virtual void InitGUIDrawOrder(List<GUIDrawAction> drawOrder)
        {
            if (drawOrder.Count != 0)
            {
                return;
            }

            drawOrder.AddRange(new[]
            {
                GUIDrawAction.DrawTags,
                GUIDrawAction.DrawAutoStart,
                GUIDrawAction.DrawProcess,
                GUIDrawAction.DrawLogger
            });
        }

        protected abstract FieldSlotInfos GetFieldSlotInfos(FieldSlot slot);
        protected void GetFieldSlotValues(FieldSlot slot, ref string validationValue)
        {
            string currentValue = string.Empty;
            FieldSlotInfos infos = GetFieldSlotInfos(slot);
            this.prefsWrapper.Get(slot, ref validationValue, validationValue);
            GetFieldSlotValues(slot, infos, ref currentValue, ref validationValue);
        }

        protected void GetFieldSlotValues(FieldSlot slot, ref string currentValue, ref string validationValue)
        {
            FieldSlotInfos infos = GetFieldSlotInfos(slot);
            this.prefsWrapper.Get(slot, ref validationValue, validationValue);
            GetFieldSlotValues(slot, infos, ref currentValue, ref validationValue);
        }

        protected void GetFieldSlotValues(FieldSlot slot, FieldSlotInfos infos, ref string validationValue)
        {
            string currentValue = string.Empty;
            this.prefsWrapper.Get(slot, ref validationValue, validationValue);
            GetFieldSlotValues(slot, infos, ref currentValue, ref validationValue);
        }

        protected abstract void GetFieldSlotValues(FieldSlot slot, FieldSlotInfos infos, ref string currentValue, ref string validationValue);
        protected abstract void RefreshDatas();
        protected abstract bool DrawGUIAction(GUIDrawAction drawAction);

        protected void DrawFieldSlots()
        {
            //Reset tag draw order
            fieldSlotDrawOrder.Clear();

            InitFieldSlotDrawOrder(fieldSlotDrawOrder);

            bool forceRefreshDatas = false;

            //Show basic datas
            bool groupPreceding = false;
            bool groupActive = false;
            foreach (FieldSlot tag in fieldSlotDrawOrder)
            {
                if (tag == FieldSlot.MAX)
                {
                    break;
                }

                FieldSlotInfos infos = GetFieldSlotInfos(tag);
                if (!infos.HasEither(FieldSlotTag.Enabled))
                {
                    continue;
                }

                if (infos.HasEither(FieldSlotTag.AddSpaceBefore))
                {
                    EditorGUILayout.Space();
                }

                //Group lines together if either tags are active
                if (!CheckFieldSlotGrouping(infos, tag, ref groupPreceding, ref groupActive))
                {
                    continue;
                }

                using (new EditorGUI.IndentLevelScope(groupActive ? ConstsEditor.ONE_INDENT : 0))
                {
                    string editedValue = string.Empty;
                    string testValue = string.Empty;

                    GetFieldSlotValues(tag, infos, ref editedValue, ref testValue);

                    bool showCheckout = false;
                    bool doExist = false;
                    bool toggledOn = true;

                    //If the value edited needs to be verified (folder, file, ....)
                    CheckTagExistence(tag, infos, testValue, ref doExist, ref toggledOn, ref showCheckout);

                    Rect rect = EditorGUILayout.GetControlRect();

                    //Change the TagSize order in the enum to customize the render order of tags
                    tagDrawSizes[(int) GUIField.Label] = -EditorGUIUtility.labelWidth;
                    tagDrawSizes[(int) GUIField.Toggle] = !infos.HasEither(FieldSlotTag.Toggleable) ? 0 : -40;
                    tagDrawSizes[(int) GUIField.Value] = 200;
                    tagDrawSizes[(int) GUIField.Checkout] = !(showCheckout && infos.HasEither(FieldSlotTag.NeedCheckout)) ? 0 : -110;
                    tagDrawSizes[(int) GUIField.Existence] = !infos.HasEither(FieldSlotTag.NeedExistenceCheck) ? 0 : -80;
                    tagDrawSizes[(int) GUIField.Clear] = -60;

                    //Split the needed rects for all the needs below
                    Rect[] rects = RectHelper.SplitX(ref rect, tagDrawSizes);
                    {
                        using (EditorGUI.ChangeCheckScope scope = new EditorGUI.ChangeCheckScope())
                        {
                            {
                                EditorGUI.LabelField(rects[(int) GUIField.Label], infos.title);

                                //Draw toggle button and store in EditorPrefs
                                DrawTagToggle(rects, infos, tag);

                                //Draw the application data path
                                DrawTagRelativeToAsset(rects, tag, infos);

                                //Our edited value
                                editedValue = EditorGUI.TextField(rects[(int) GUIField.Value], GUIContent.none, editedValue);

                                //If the value edited needs to be verified (folder, file, ....)
                                DrawTagExistenceCheck(rects, infos, testValue, doExist, toggledOn, showCheckout);

                                //Do we want to reset the value ?
                                DrawTagClearButton(rects, infos, tag, editedValue, scope, ref forceRefreshDatas);
                            }
                        }
                    }
                }
            }

            if (forceRefreshDatas)
            {
                RefreshDatas();
            }
        }

        protected bool CheckFieldSlotGrouping(FieldSlotInfos infos, FieldSlot slot, ref bool groupPreceding, ref bool groupActive)
        {
            if (infos.HasEither(FieldSlotTag.GroupStart | FieldSlotTag.Group))
            {
                if (infos.HasEither(FieldSlotTag.GroupStart))
                {
                    groupPreceding = true;

                    string foldoutTag = slot + FieldSlotTag.GroupStart.ToString();
                    groupActive = false;
                    prefsWrapper.Get(foldoutTag, ref groupActive);

                    using (EditorGUI.ChangeCheckScope scope = new EditorGUI.ChangeCheckScope())
                    {
                        groupActive = EditorGUILayout.Foldout(groupActive, infos.title, true);
                        if (scope.changed)
                        {
                            prefsWrapper.Set(foldoutTag, groupActive);
                        }
                    }
                }

                if (!groupPreceding)
                {
                    Debug.Assert(false, $"Found {FieldSlotTag.Group} without preceding {FieldSlotTag.GroupStart}");
                }

                if (!groupActive)
                {
                    return false;
                }
            }
            else if (groupActive)
            {
                groupPreceding = false;
                groupActive = false;
            }

            return true;
        }

        protected void DrawTagToggle(Rect[] rects, FieldSlotInfos infos, FieldSlot slot)
        {
            if (infos.HasEither(FieldSlotTag.Toggleable))
            {
                bool toggle = infos.HasEither(FieldSlotTag.ToggleDefaultOn);
                prefsWrapper.Get(slot, ref toggle);
                using (EditorGUI.ChangeCheckScope toggleScope = new EditorGUI.ChangeCheckScope())
                {
                    toggle = EditorGUI.ToggleLeft(rects[(int) GUIField.Toggle], GUIContent.none, toggle);

                    if (toggleScope.changed)
                    {
                        prefsWrapper.Set(slot, toggle);
                    }
                }
            }
        }

        protected void DrawTagRelativeToAsset(Rect[] rects, FieldSlot slot, FieldSlotInfos infos)
        {
            if (infos.HasEither(FieldSlotTag.RelativeToAssetPath | FieldSlotTag.RelativeToOtherPath))
            {
                string path = infos.HasEither(FieldSlotTag.RelativeToAssetPath) ? Application.dataPath : GetRelativePath(slot, infos);
                Rect pathRect = rects[(int) GUIField.Value];
                Rect[] pathRects = RectHelper.SplitX(ref pathRect, Split.FixedSize(10 + GUI.skin.label.CalcSize(new GUIContent(path)).x), 100);
                rects[(int) GUIField.Value] = pathRects[1];

                EditorGUI.LabelField(pathRects[0], path);
            }
        }

        protected void CheckTagExistence(FieldSlot slot, FieldSlotInfos infos, string testValue, ref bool doExist, ref bool toggledOn, ref bool showCheckout)
        {
            if (infos.HasEither(FieldSlotTag.NeedExistenceCheck))
            {
                if (infos.HasEither(FieldSlotTag.IsType))
                {
                    doExist = AssemblyHelper.GetTypeFromAnyAssembly(testValue) != null;
                }
                else if (infos.HasEither(FieldSlotTag.IsDirectory))
                {
                    doExist = Directory.Exists(testValue);
                }
                else if (infos.HasEither(FieldSlotTag.IsFile))
                {
                    doExist = File.Exists(testValue);
                }

                if (infos.HasEither(FieldSlotTag.Toggleable))
                {
                    prefsWrapper.Get(slot, ref toggledOn);
                }

                if (doExist && infos.HasEither(FieldSlotTag.NeedCheckout) && infos.HasEither(FieldSlotTag.IsFile))
                {
                    FileAttributes attributes = File.GetAttributes(testValue);
                    if ((attributes & FileAttributes.ReadOnly) != 0)
                    {
                        showCheckout = true;
                    }
                }
            }
        }

        protected void DrawTagExistenceCheck(Rect[] rects, FieldSlotInfos infos, string testValue, bool doExist, bool toggledOn, bool showCheckout)
        {
            if (infos.HasEither(FieldSlotTag.NeedExistenceCheck))
            {
                if (showCheckout)
                {
                    if (GUI.Button(rects[(int) GUIField.Checkout], "Show in Perforce"))
                    {
                        SourceControlHelper.SelectInPerforce(testValue);
                    }

                    GUIHelper.ShowStatusBox(rects[(int) GUIField.Existence], false, string.Empty, string.Empty, "READ ONLY");
                }
                else
                {
                    GUIHelper.ShowStatusBox(rects[(int) GUIField.Existence], doExist);

                    if (toggledOn)
                    {
                        RefreshStatusOnFileExist(doExist);
                    }
                }
            }
        }

        protected void DrawTagClearButton(Rect[] rects, FieldSlotInfos infos, FieldSlot slot, string editedValue, EditorGUI.ChangeCheckScope scope, ref bool forceRefreshDatas)
        {
            if (GUI.Button(rects[(int) GUIField.Clear], new GUIContent("Clear")))
            {
                prefsWrapper.ClearKey<string>(slot.ToString());
                forceRefreshDatas = true;
            }
            else if (scope.changed)
            {
                SetFieldSlotTo(slot, editedValue);
                prefsWrapper.Set(slot, editedValue);
            }
        }
        #endregion Main GUI Logic
    }
}
