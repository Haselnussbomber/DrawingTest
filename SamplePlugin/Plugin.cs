using Dalamud.Game;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using Lumina;
using Lumina.Data;
using Lumina.Excel;
using Lumina.Text;
using Lumina.Text.Payloads;
using Lumina.Text.ReadOnly;

namespace SamplePlugin;

public sealed class Plugin : IDalamudPlugin
{
    [PluginService] internal static IDalamudPluginInterface PluginInterface { get; private set; } = null!;
    [PluginService] internal static IDataManager DataManager { get; private set; } = null!;
    [PluginService] internal static IPluginLog PluginLog { get; private set; } = null!;

    private readonly TextDecoder textDecoder;

    public Plugin()
    {
        textDecoder = new(PluginLog, DataManager);

        var sheet = DataManager.Excel.GetSheetRaw("quest/045/BanArk110_04560");
        if (sheet == null)
        {
            PluginLog.Warning("Sheet not found!");
            return;
        }

        foreach (var rowParser in sheet)
        {
            if (rowParser.RowId != 24) continue;

            var questText = new QuestText();
            questText.PopulateData(rowParser, DataManager.GameData, Language.German);

            PluginLog.Debug("{key} = {value} => {parsed}", questText.Key.ExtractText(), questText.Value.ToString(), ProcessString(questText.Value).ExtractText());

            break;
        }
    }

    // Veeeeery simple SeString evaluator that is missing a lot of things
    private ReadOnlySeString ProcessString(ReadOnlySeString input)
    {
        var sb = new SeStringBuilder();

        foreach (var payload in input)
        {
            switch (payload.Type)
            {
                case ReadOnlySePayloadType.Text:
                    sb.Append(payload);
                    break;

                case ReadOnlySePayloadType.Macro:
                    switch (payload.MacroCode)
                    {
                        case MacroCode.Head:
                            {
                                if (!payload.TryGetExpression(out var expr1))
                                    break;

                                if (!expr1.TryGetString(out var text))
                                    break;

                                var str = ProcessString(text).ExtractText();
                                if (str.Length == 0)
                                    break;

                                sb.Append(str[..1].ToUpperInvariant());
                                sb.Append(str[1..]);
                                break;
                            }

                        case MacroCode.DeNoun:
                            {
                                if (!payload.TryGetExpression(out var expr1, out var expr2, out var expr3, out var expr4, out var expr5))
                                    break;

                                if (!expr1.TryGetString(out var rawSheetName))
                                    break;

                                if (!expr2.TryGetInt(out var person))
                                    break;

                                if (!expr3.TryGetInt(out var rowId))
                                    break;

                                if (!expr4.TryGetInt(out var amount))
                                    break;

                                if (!expr5.TryGetInt(out var @case))
                                    break;

                                var sheetName = rawSheetName.ExtractText();

                                if (sheetName == "EObj")
                                    sheetName = "EObjName";

                                sb.Append(textDecoder.ProcessNoun(ClientLanguage.German, sheetName, person, rowId, amount, @case));
                                break;
                            }

                        default:
                            sb.Append(payload);
                            break;
                    }
                    break;
            }
        }

        return sb.ToReadOnlySeString();
    }

    public void Dispose()
    {

    }
}

public class QuestText : ExcelRow
{
    public ReadOnlySeString Key { get; set; }
    public ReadOnlySeString Value { get; set; }

    public override void PopulateData(RowParser parser, GameData gameData, Language language)
    {
        base.PopulateData(parser, gameData, language);

        var key = parser.ReadColumn<SeString>(0)!;
        var value = parser.ReadColumn<SeString>(1)!;

        Key = new(key.RawData.ToArray());
        Value = new(value.RawData.ToArray());
    }
}
