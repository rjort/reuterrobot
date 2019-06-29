using System;
using System.Collections.Generic;
using System.Text;
using Robocode;
using System.Drawing;
using Robocode.Util;



namespace TRB2
{
    class Vasolino_2 : AdvancedRobot
    {
        double poderTiro = 1;
        int tipoTiro = 1;

        double armaDiretoDisparo = 1;
        double armaDiretoAcerto = 1;
        double armaFrenteDisparo = 0;
        double armaFrenteAcerto = 0;
        double armaDisparo = 0;
        double armaAcertar = 0;

        double energia = 100;
        double trocaEnergia = 0;

        double direcao = 136;
        double distanciaVerifica = 0;

        int d = 1;
        int radarDistancia = 1;

        public new bool IsAdjustGunForRobotTurn { get { return true; } set { bool val = true; } }
        public new bool IsAdjustRadarForGunTurn { get { return true; } set { bool val = true; } }
        public new bool IsAdjustRadarForRobotTurn { get { return true; } set { bool val = true ; } }

        public override void Run()
        {
           
            SetColors(Color.Black, Color.Yellow, Color.Red);

            while (true)
            {
                SetTurnRadarRight(45 * radarDistancia);
                Execute();
            }

        } // run

        public override void OnScannedRobot(ScannedRobotEvent e)
        {
            if (Velocity == 0)
            {
                SetAhead(direcao = -direcao);
            }
            if (e.Distance > 400)
            {
                SetTurnRightRadians(e.BearingRadians + (Math.PI / 2) - (Math.PI / 6) * direcao / 137);
            }
            else
            {
                SetTurnRightRadians(e.BearingRadians + (Math.PI / 2) + (Math.PI / 6) * direcao / 137);
            }

            //verifica o tipo de tiro a ser disparado
            trocaEnergia = energia - e.Energy;
            if (trocaEnergia > 0 && trocaEnergia <= 3)
            {
                direcao = -direcao;
            }

            radarDistancia = -radarDistancia;
            SetTurnRadarRight(999999 * radarDistancia);
            if (e.Distance < 600)
            {
                if (armaDisparo <= 10 || armaFrenteAcerto / armaFrenteDisparo >= armaDiretoAcerto / armaDiretoDisparo)
                {
                    tipoTiro = 1;
                    double absoluteBearing = (HeadingRadians + e.BearingRadians);
                    SetTurnGunRightRadians(Utils.NormalRelativeAngle(absoluteBearing - GunHeadingRadians + (e.Velocity * Math.Sin(e.HeadingRadians - absoluteBearing) / 13.0)));
                }
                else
                {
                    tipoTiro = 2;
                    SetTurnGunRightRadians(Utils.NormalRelativeAngle(HeadingRadians + e.BearingRadians - GunHeadingRadians));
                }

                poderTiro = (3 - (3 * (e.Distance / 750)));
                SetFire(poderTiro);
            }

            energia = e.Energy;
            Scan();
        } // onscannedrobot

        public override void OnHitByBullet(HitByBulletEvent e)
        {
            armaDisparo += 1;
            if (tipoTiro == 1)
            {
                armaFrenteAcerto += 1;
                armaFrenteDisparo += 1;
            }
            else if (tipoTiro == 2)
            {
                armaDiretoDisparo += 1;
                armaDiretoAcerto += 1;
            }
        } //onhitbybullet

        public override void OnBulletMissed(BulletMissedEvent e)
        {
            armaDisparo += 1;
            if (tipoTiro == 1)
            {
                armaFrenteDisparo += 1;
            }
            else if (tipoTiro == 2)
            {
                armaDiretoDisparo += 1;
            }
        } // onbulletmiss

        public override void OnHitWall(HitWallEvent e)
        {
            direcao = -direcao;
        } // onhitwall

        public override void OnWin(WinEvent e)
        {
            TurnLeft(15);
            TurnRight(15);
        } // onwin
    } // class

} // namespace