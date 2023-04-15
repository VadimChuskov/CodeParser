using System.Text;
using CodeParser.Domain;
using CodeParser.Logic;
using CodeParser.Logic.Interfaces;

namespace CodeParser
{
    public partial class Form1 : Form
    {
        private readonly IFileParserService _fileParserService;

        public Form1()
        {
            InitializeComponent();
            _fileParserService = new FileParserService();
        }

        private async void btn_ParseFile_Click(object sender, EventArgs e)
        {
            //var path = "C:\\Work\\StyleRow\\stylerow\\StyleRow.Core\\Audit\\BusinessObjects\\ActionType.cs";
            var path = "c:\\Work\\StyleRow\\stylerow\\StyleRow.Logic\\Project\\ProjectItemService.cs";
            var result = await _fileParserService.ParseAsync(path);
            ;
        }

        private async void btn_InitModel_Click(object sender, EventArgs e)
        {
            var path = "C:\\Work\\StyleRow\\stylerow\\StyleRow.DataContracts\\Project\\ProjectSettingsTaxDto.cs";
            var result = await _fileParserService.ParseAsync(path);
            var item = result.FirstOrDefault() as Class;

            if (item == null) { return; }

            var initValue = new StringBuilder();
            initValue.Append($"var {item.Name[0].ToString().ToLower()}{item.Name.Substring(1)} = new {item.Name}\r\n");
            initValue.Append("{\r\n");
            var index = 1;
            foreach (var field in item.Fields)
            {
                initValue.Append($"\t{field.Name} = ");

                if (!field.IsValueType)
                {
                    initValue.Append($"new {field.Type}(),\r\n");
                }
                else
                {
                    switch (field.Type)
                    {
                        case "bool":
                            initValue.Append("true,\r\n");
                            break;
                        case "string":
                            initValue.Append($"\"{field.Parent.Name}_{field.Name}\",\r\n");
                            break;
                        case "int":
                            initValue.Append($"{index++},\r\n");
                            break;
                        case "float":
                            initValue.Append($"{index + index++/10},\r\n");
                            break;
                        default:
                            initValue.Append($"{field.Name},\r\n");
                            break;
                    }
                }


            }
            initValue.Append("};\r\n");
            var resultA = initValue.ToString();
            ;
        }
    }
}