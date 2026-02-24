using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProductsImporter
{
    public partial class FrmInportListini : Form
    {
        private readonly ConfigService _configService;
        private readonly LoggerService _logger;
        private readonly ExcelReader _excelReader;
        private readonly ArticoliRepository _artRepo;
        private readonly ListiniRepository _listRepo;
        private readonly ImportService _importService;

        public FrmInportListini()
        {
            InitializeComponent();

            _configService = new ConfigService();
            _logger = new LoggerService();
            _excelReader = new ExcelReader();
            _artRepo = new ArticoliRepository();
            _listRepo = new ListiniRepository();

            _configService.Load();

            _importService = new ImportService(_excelReader, _artRepo, _listRepo, _configService, _logger);

            _logger.OnLog += (s) =>
            {
                if (txtLog.InvokeRequired)
                    txtLog.BeginInvoke(new Action(() => { txtLog.AppendText(s + Environment.NewLine); }));
                else
                    txtLog.AppendText(s + Environment.NewLine);
            };
            AbilitaDisabilitaControlli();
        }

        private void btnSfoglia_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtPercorsoFile.Text = openFileDialog1.FileName;
            }

            AbilitaDisabilitaControlli();
        }
        private void AbilitaDisabilitaControlli()
        {
            btnImportaListino.Enabled = !string.IsNullOrWhiteSpace(txtPercorsoFile.Text);
            ChkCreaArticoli.Enabled = !string.IsNullOrWhiteSpace(txtPercorsoFile.Text);
            ChkImportaPrezzi.Enabled = !string.IsNullOrWhiteSpace(txtPercorsoFile.Text);
        }

        private async void btnImportaListino_Click(object sender, EventArgs e)
        {
            var file = txtPercorsoFile.Text;
            if (string.IsNullOrWhiteSpace(file))
            {
                MessageBox.Show("Selezionare un file .xlsx", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnImportaListino.Enabled = false;
            txtLog.Clear();
            progressBarImport.Value = 0;

            var progress = new Progress<int>(p =>
            {
                progressBarImport.Value = Math.Min(100, Math.Max(0, p));
            });

            try
            {
                _logger.Log($"Inizio import: {file}");
                var result = await _importService.ImportAsync(file, ChkCreaArticoli.Checked, ChkImportaPrezzi.Checked, progress);

                _logger.Log($"Import completato. Totale: {result.TotalRows}, Inseriti: {result.Inserted}, Aggiornati: {result.Updated}, Errori: {result.Errors}");
                if (result.ErrorDetails.Count > 0)
                {
                    _logger.Log("Dettaglio errori:");
                    foreach (var eDet in result.ErrorDetails)
                        _logger.Log(eDet);
                }

                MessageBox.Show("Import completato. Controllare il log per i dettagli.", "Fine", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                _logger.Log("Errore durante l'importazione: " + ex.Message);
                MessageBox.Show("Errore: " + ex.Message, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnImportaListino.Enabled = true;
            }
        }
    }
}
