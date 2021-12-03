using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MiniGame
{
    class Ctverecek
    {
        private GraphicsDevice _zobrazovac { get; set; }

        private int _velikost { get; set; }
        private float _rychlost { get; set; }

        private float _rychlostKlepani { get; set; }

        private Color _barva { get; set; }

        private Vector2 _pozice { get; set; }
        private Texture2D _textura { get; set; }

        private SmeroveOvladani _ovladaniPohybu { get; set; }

        public Ctverecek(int velikost, float rychlost, float rychlostKlepani, Vector2 pozice, SmeroveOvladani ovladaniPohybu, Rectangle omezeniPohybu, Color barva, GraphicsDevice zobrazovac)
        {
            _velikost = velikost;
            _rychlost = rychlost;
            _rychlostKlepani = rychlostKlepani;

            _ovladaniPohybu = ovladaniPohybu;

            _barva = barva;
            _pozice = pozice;

            _zobrazovac = zobrazovac;
            _textura = PripravitTexturu();
        }

        private Texture2D PripravitTexturu()
        {
            Texture2D vyslednaTextura = new Texture2D(_zobrazovac, _velikost, _velikost);

            Color[] pixely = new Color[_velikost * _velikost];
            for (int i = 0; i < pixely.Length; i++)
                pixely[i] = Color.White;
            vyslednaTextura.SetData(pixely);
            
            return vyslednaTextura;
        }

        private void Pohnout(KeyboardState klavesnice)
        {
            Vector2 smerPohybu = Vector2.Zero;

            if (klavesnice.IsKeyDown(_ovladaniPohybu.Doprava))
                smerPohybu += Vector2.UnitX;
            if (klavesnice.IsKeyDown(_ovladaniPohybu.Doleva))
                smerPohybu -= Vector2.UnitX;
            if (klavesnice.IsKeyDown(_ovladaniPohybu.Nahoru))
                smerPohybu -= Vector2.UnitY;
            if (klavesnice.IsKeyDown(_ovladaniPohybu.Dolu))
                smerPohybu += Vector2.UnitY;

            if (smerPohybu != Vector2.Zero)
                _pozice += _rychlost * Vector2.Normalize(smerPohybu);
        }


        public void Aktualizovat(KeyboardState klavesnice)
        {

            Pohnout(klavesnice);
            Zatresat(klavesnice);
        }

        public void Vykreslit(SpriteBatch _vykreslovac)
        {
            _vykreslovac.Draw(_textura, _pozice, _barva);
        }
        private void Zatresat(KeyboardState klavesnice)
        {
            float zvyseniRychlosti = 0.01F;
            float zmenseniRychlosti = 0.5F;
            int pocetZmacknutychKlaves = klavesnice.GetPressedKeyCount();

            if (pocetZmacknutychKlaves != 0)
            {
                _rychlost += zvyseniRychlosti;
                _rychlostKlepani += zvyseniRychlosti;
                TresatDoStran();
            }
            if (pocetZmacknutychKlaves == 0 && _rychlost > 5)
            {
                _rychlost -= zmenseniRychlosti;
                _rychlostKlepani -= zmenseniRychlosti;
            }
        }
        private void TresatDoStran()
        {
            Random random = new Random();
            Vector2 smerPohybu = Vector2.Zero;
            int nahodnaMoznost = random.Next(1, 5);

            switch (nahodnaMoznost)
            {
                case 1:
                    smerPohybu -= Vector2.UnitY;
                    break;
                case 2:
                    smerPohybu += Vector2.UnitX;
                    break;
                case 3:
                    smerPohybu += Vector2.UnitY;
                    break;
                case 4:
                    smerPohybu -= Vector2.UnitX;
                    break;
            }
            if (smerPohybu != Vector2.Zero)
            {
                _pozice += _rychlostKlepani * Vector2.Normalize(smerPohybu);
            }
        }
    }
}
