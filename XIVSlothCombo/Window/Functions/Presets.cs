using System.Linq;
using System.Numerics;
using System.Text;
using Dalamud.Interface.Colors;
using Dalamud.Utility;
using ImGuiNET;
using XIVSlothCombo.Attributes;
using XIVSlothCombo.Combos;
using XIVSlothCombo.Core;
using XIVSlothCombo.Data;
using XIVSlothCombo.Extensions;
using XIVSlothCombo.Services;
using SysGeneric = System.Collections.Generic;

namespace XIVSlothCombo.Window.Functions
{
    internal class Presets : ConfigWindow
    {
        internal static void DrawPreset(CustomComboPreset preset, CustomComboInfoAttribute info, ref int i)
        {
            bool enabled = Service.Configuration.IsEnabled(preset);
            bool secret = PluginConfiguration.IsSecret(preset);
            CustomComboPreset[] conflicts = Service.Configuration.GetConflicts(preset);
            CustomComboPreset? parent = PluginConfiguration.GetParent(preset);
            BlueInactiveAttribute? blueAttr = preset.GetAttribute<BlueInactiveAttribute>();

            ImGui.PushItemWidth(200);

            if (ImGui.Checkbox($"{info.FancyName}###{info.FancyName}{i}", ref enabled))
            {
                if (enabled)
                {
                    EnableParentPresets(preset);
                    Service.Configuration.EnabledActions.Add(preset);
                    foreach (CustomComboPreset conflict in conflicts)
                    {
                        Service.Configuration.EnabledActions.Remove(conflict);
                    }
                }

                else
                {
                    Service.Configuration.EnabledActions.Remove(preset);
                }

                Service.Configuration.Save();
            }

            ImGui.PopItemWidth();
            ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.DalamudGrey);

            DrawOpenerButtons(preset);

            Vector2 length = new();

            if (i != -1)
            {
                ImGui.Text($"#{i}: ");
                length = ImGui.CalcTextSize($"#{i}: ");
                ImGui.SameLine();
                ImGui.PushItemWidth(length.Length());
            }

            ImGui.TextWrapped($"{info.Description}");

            if (preset.GetHoverAttribute() != null)
            {
                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();
                    ImGui.TextUnformatted(preset.GetHoverAttribute().HoverText);
                    ImGui.EndTooltip();
                }
            }


            ImGui.PopStyleColor();
            ImGui.Spacing();

            UserConfigItems.Draw(preset, enabled);

            if (preset is CustomComboPreset.NIN_ST_SimpleMode_BalanceOpener or CustomComboPreset.NIN_ST_AdvancedMode_BalanceOpener)
            {
                ImGui.SetCursorPosX(ImGui.GetCursorPosX() + length.Length());
                if (ImGui.Button($"Image of rotation###ninrtn{i}"))
                {
                    Util.OpenLink("https://i.imgur.com/q3lXeSZ.png");
                }
            }

            if (conflicts.Length > 0)
            {
                string conflictText = conflicts.Select(conflict =>
                {
                    CustomComboInfoAttribute? conflictInfo = conflict.GetComboAttribute();

                    return $"\n - {conflictInfo.FancyName}";


                }).Aggregate((t1, t2) => $"{t1}{t2}");

                if (conflictText.Length > 0)
                {
                    ImGui.TextColored(ImGuiColors.DalamudRed, $"Conflicts with: {conflictText}");
                    ImGui.Spacing();
                }
            }

            if (blueAttr != null)
            {
                if (blueAttr.Actions.Count > 0)
                {
                    ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.DalamudOrange);
                    ImGui.Text($"Missing active spells: {string.Join(", ", blueAttr.Actions.Select(x => ActionWatching.GetActionName(x)))}");
                    ImGui.PopStyleColor();
                }

                else
                {
                    ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.HealerGreen);
                    ImGui.Text($"All required spells active!");
                    ImGui.PopStyleColor();
                }
            }

            VariantParentAttribute? varientparents = preset.GetAttribute<VariantParentAttribute>();
            if (varientparents is not null)
            {
                ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.HealerGreen);
                ImGui.TextWrapped($"Part of normal combo{(varientparents.ParentPresets.Length > 1 ? "s" : "")}:");
                StringBuilder builder = new();
                foreach (CustomComboPreset par in varientparents.ParentPresets)
                {
                    builder.Insert(0, $"{par.GetAttribute<CustomComboInfoAttribute>().FancyName}");
                    CustomComboPreset par2 = par;
                    while (PluginConfiguration.GetParent(par2) != null)
                    {
                        CustomComboPreset? subpar = PluginConfiguration.GetParent(par2)!;
                        builder.Insert(0, $"{subpar.GetAttribute<CustomComboInfoAttribute>().FancyName} -> ");
                        par2 = subpar.Value;

                    }

                    ImGui.TextWrapped($"- {builder}");
                    builder.Clear();
                }
                ImGui.PopStyleColor();
            }

            BozjaParentAttribute? bozjaparents = preset.GetAttribute<BozjaParentAttribute>();
            if (bozjaparents is not null)
            {
                ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.HealerGreen);
                ImGui.TextWrapped($"Part of normal combo{(varientparents.ParentPresets.Length > 1 ? "s" : "")}:");
                StringBuilder builder = new();
                foreach (CustomComboPreset par in bozjaparents.ParentPresets)
                {
                    builder.Insert(0, $"{par.GetAttribute<CustomComboInfoAttribute>().FancyName}");
                    CustomComboPreset par2 = par;
                    while (PluginConfiguration.GetParent(par2) != null)
                    {
                        CustomComboPreset? subpar = PluginConfiguration.GetParent(par2)!;
                        builder.Insert(0, $"{subpar.GetAttribute<CustomComboInfoAttribute>().FancyName} -> ");
                        par2 = subpar.Value;

                    }

                    ImGui.TextWrapped($"- {builder}");
                    builder.Clear();
                }
                ImGui.PopStyleColor();
            }

            EurekaParentAttribute? eurekaparents = preset.GetAttribute<EurekaParentAttribute>();
            if (eurekaparents is not null)
            {
                ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.HealerGreen);
                ImGui.TextWrapped($"Part of normal combo{(varientparents.ParentPresets.Length > 1 ? "s" : "")}:");
                StringBuilder builder = new();
                foreach (CustomComboPreset par in eurekaparents.ParentPresets)
                {
                    builder.Insert(0, $"{par.GetAttribute<CustomComboInfoAttribute>().FancyName}");
                    CustomComboPreset par2 = par;
                    while (PluginConfiguration.GetParent(par2) != null)
                    {
                        CustomComboPreset? subpar = PluginConfiguration.GetParent(par2)!;
                        builder.Insert(0, $"{subpar.GetAttribute<CustomComboInfoAttribute>().FancyName} -> ");
                        par2 = subpar.Value;

                    }

                    ImGui.TextWrapped($"- {builder}");
                    builder.Clear();
                }
                ImGui.PopStyleColor();
            }

            i++;

            bool hideChildren = Service.Configuration.HideChildren;
            (CustomComboPreset Preset, CustomComboInfoAttribute Info)[] children = presetChildren[preset];

            if (children.Length > 0)
            {
                if (enabled || !hideChildren)
                {
                    ImGui.Indent();

                    foreach ((CustomComboPreset childPreset, CustomComboInfoAttribute childInfo) in children)
                    {
                        if (Service.Configuration.HideConflictedCombos)
                        {
                            CustomComboPreset[] conflictOriginals = Service.Configuration.GetConflicts(childPreset);        // Presets that are contained within a ConflictedAttribute
                            SysGeneric.List<CustomComboPreset> conflictsSource = Service.Configuration.GetAllConflicts();   // Presets with the ConflictedAttribute

                            if (!conflictsSource.Where(x => x == childPreset || x == preset).Any() || conflictOriginals.Length == 0)
                            {
                                DrawPreset(childPreset, childInfo, ref i);
                                continue;
                            }

                            if (conflictOriginals.Any(x => Service.Configuration.IsEnabled(x)))
                            {
                                Service.Configuration.EnabledActions.Remove(childPreset);
                                Service.Configuration.Save();
                            }

                            else
                            {
                                DrawPreset(childPreset, childInfo, ref i);
                                continue;
                            }
                        }

                        else
                        {
                            DrawPreset(childPreset, childInfo, ref i);
                        }
                    }

                    ImGui.Unindent();
                }
                else
                {
                    i += AllChildren(presetChildren[preset]);

                }
            }
        }

        private static void DrawOpenerButtons(CustomComboPreset preset)
        {
            if (preset.GetReplaceAttribute() != null)
            {
                string skills = string.Join(", ", preset.GetReplaceAttribute().ActionNames);

                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();
                    ImGui.TextUnformatted($"Replaces: {skills}");
                    ImGui.EndTooltip();
                }
            }
        }

        internal static int AllChildren((CustomComboPreset Preset, CustomComboInfoAttribute Info)[] children)
        {
            int output = 0;

            foreach ((CustomComboPreset Preset, CustomComboInfoAttribute Info) in children)
            {
                output++;
                output += AllChildren(presetChildren[Preset]);
            }

            return output;
        }



        /// <summary> Iterates up a preset's parent tree, enabling each of them. </summary>
        /// <param name="preset"> Combo preset to enabled. </param>
        private static void EnableParentPresets(CustomComboPreset preset)
        {
            CustomComboPreset? parentMaybe = PluginConfiguration.GetParent(preset);

            while (parentMaybe != null)
            {
                CustomComboPreset parent = parentMaybe.Value;

                if (!Service.Configuration.EnabledActions.Contains(parent))
                {
                    Service.Configuration.EnabledActions.Add(parent);

                    foreach (CustomComboPreset conflict in Service.Configuration.GetConflicts(parent))
                    {
                        Service.Configuration.EnabledActions.Remove(conflict);
                    }
                }

                parentMaybe = PluginConfiguration.GetParent(parent);
            }
        }
    }
}
