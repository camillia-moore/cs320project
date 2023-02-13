using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MonoGame.UI.Forms;

namespace GameStateTesting
{
    class MyButtons : ControlManager
    {
        public MyButtons (Game1 game) : base (game)
        {


        }

        public override void InitializeComponent()
        {
            var btn1 = new Button()
            {
                Text = "TEST!",
                Size = new Vector2(200,50),
                BackgroundColor = Color.DarkMagenta
            };

            btn1.Clicked += Btn1_Clicked;
            Controls.Add(btn1);
            //throw new NotImplementedException();
        }

        private void Btn1_Clicked(object sender, EventArgs e)
        {
            Button btn = sender as Button; // (Button)sender;
            btn.Text = "Clicked!";
        }
    }
}
