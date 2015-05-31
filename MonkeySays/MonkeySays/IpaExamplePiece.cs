using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.UI.Text;

namespace MonkeySays
{
    class IpaExamplePiece
    {
        public IpaExamplePiece(string piece, bool isBold)
        {
            Piece = piece;
            Weight = isBold ? FontWeights.Bold : FontWeights.Normal;
        }

        public string Piece
        {
            get;
            private set;
        }

        public FontWeight Weight
        {
            get;
            private set;
        }
    }
}
