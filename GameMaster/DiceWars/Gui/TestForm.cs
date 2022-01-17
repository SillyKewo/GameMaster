using System;
using System.Drawing;
using System.Windows.Forms;
using Hexagonal;

namespace HexagonalTest
{
    public partial class Fight : Form
    {
        private Board board;
        private GraphicsEngine graphicsEngine;
        private int sizeOfField;

        public Fight(Board x, int size)
        {
            board = x;
            sizeOfField = size;

            InitializeComponent(sizeOfField);

            startGame();
        }

        private void Form_MouseMove(object sender, MouseEventArgs e)
        {
            if (graphicsEngine == null)
            {
                return;
            }

            Point relativeMouse = new Point(e.X - graphicsEngine.BoardXOffset, e.Y - graphicsEngine.BoardYOffset);
            Hex hex = board.FindHexMouseClick(relativeMouse);

            labelXY.Text = $"[{hex?.GridPositionX}, {hex?.GridPositionY}] ({relativeMouse.X}, {relativeMouse.Y})";
        }

        private void startGame()
        {
            this.graphicsEngine = new GraphicsEngine(board, 20, 20);
        }

        private void Form_MouseClick(object sender, MouseEventArgs e)
        {
            //Console.WriteLine("Mouse Click " + e.Location.ToString());

            if (board != null && graphicsEngine != null)
            {
                //
                // need to account for any offset
                //
                Point mouseClick = new Point(e.X - graphicsEngine.BoardXOffset, e.Y - graphicsEngine.BoardYOffset);

                //Console.WriteLine("Click in Board bounding rectangle: {0}", board.PointInBoardRectangle(e.Location));

                Hex clickedHex = board.FindHexMouseClick(mouseClick);

                if (clickedHex == null)
                {
                    Console.WriteLine("No hex was clicked.");
                }
                else
                {
                    if (board.getCurrentPlayerColor() == clickedHex.HexState.BackgroundColor)
                    {
                        /*bool canMoveDices = true;
                        if (e.Button == MouseButtons.Right)
                        {
                            Console.WriteLine("Hex " + clickedHex + " was clicked." + "Hex has " + clickedHex.Dices + " Dices");
                            canMoveDices = board.moveDices(board.BoardState.ActiveHex, clickedHex);
                        }*/
                        board.BoardState.ActiveHex = clickedHex;
                        labelAttacker.BackColor = board.getCurrentPlayerColor();
                        labelDefender.BackColor = Color.LightGray;
                    }
                    else if (board.CanAttack(board.BoardState.ActiveHex, clickedHex, out _))
                    {
                        var res = board.PerformAttack(board.BoardState.ActiveHex, clickedHex);

                        labelAttackerDices.Text = res.Item2.ToString();
                        labelDefenderDices.Text = res.Item3.ToString();

                        Player attackerP = board.findPlayerByColor(board.BoardState.ActiveHex.HexState.BackgroundColor);
                        if (board.HasWon(attackerP))
                        {
                            //Triggered if player has won
                            Console.WriteLine(attackerP.Color.Name + " has won.");
                            HexagonalTest.GameOver gameOverForm = new HexagonalTest.GameOver(attackerP.Color.Name);
                            gameOverForm.Show();
                        }
                    }
                }
            }
            //Update status lable
            lable_players.Text = this.board.getStatus();
        }

        private void Form_Paint(object sender, PaintEventArgs e)
        {
            //Draw the graphics/GUI

            foreach (Control c in this.Controls)
            {
                c.Refresh();
            }

            if (graphicsEngine != null)
            {
                graphicsEngine.Draw(e.Graphics);
            }
            //set Current player from model
            current_player.BackColor = this.board.getCurrentPlayerColor();
            //Update status lable
            lable_players.Text = this.board.getStatus();
            //Force the next Paint()
            this.Invalidate();
        }

        private void Form_Closing(object sender, FormClosingEventArgs e)
        {
            if (graphicsEngine != null)
            {
                graphicsEngine = null;
            }

            if (board != null)
            {
                board = null;
            }

            Application.Exit();
        }

        private void bt_end_Click(object sender, EventArgs e)
        {
            this.board.nextPlayer();
            
            //set Current player from model
            current_player.BackColor = this.board.getCurrentPlayerColor();
        }
    }
}