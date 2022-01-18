using System.Drawing;
using HexagonalTest.PlayerAPI;

namespace Hexagonal
{
    public class Player
    {
        private int id;
        private int fields;
        private Color color;

        public Player(int id, Color color, IDiceWarsPlayerLogic playerLogic)
        {
            this.id = id;
            this.fields = 0;
            this.color = color;
            this.PlayerLogic = playerLogic;
        }

        public int ID
        {
            get
            {
                return id;
            }
            internal set
            {
                this.id = value;
            }
        }

        public Color Color
        {
            get
            {
                return color;
            }
            internal set
            {
                this.color = value;
            }
        }

        public int Fields
        {
            get
            {
                return fields;
            }
            set
            {
                this.fields = value;
            }
        }

        public int Reward { get; set; }
        public IDiceWarsPlayerLogic PlayerLogic { get; }

        public void addField()
        {
            this.fields++;
        }

        internal void removeField()
        {
            if (fields > 0)
            {
                this.fields--;
            }
        }
    }
}