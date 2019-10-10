using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MedocUpdates
{
    static class Program
    {
		static Log log = new Log();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
			log.Write("MedocUpdates: Initializing...");

			Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());

			log.Write("MedocUpdates: Shutting down the application");
		}
    }
}
