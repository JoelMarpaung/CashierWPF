using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRUDBootcamp32
{
    public partial class TransactionReportForm : Form
    {
        int docKey;
        public TransactionReportForm(int docId)
        {
            InitializeComponent();
            this.docKey = docId;
        }

        private void TransactionReportForm_Load(object sender, EventArgs e)
        {
            TransactionReport1.SetParameterValue("@DocKey", docKey);
        }
    }
}
