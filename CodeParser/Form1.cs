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
    }
}