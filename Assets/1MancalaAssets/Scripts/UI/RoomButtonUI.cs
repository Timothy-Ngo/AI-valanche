/***************************************************************************\
Project:      Mancala
Copyright (c) Niobium Studios.
Author:       Guilherme Nunes Barbosa (gnunesb@gmail.com)
\***************************************************************************/
using UnityEngine;
using UnityEngine.UI;

namespace Niobium
{
    /**
    * Dynamic Room Button UI
    **/
    public class RoomButtonUI : MonoBehaviour
    {
        [Header("UI Elements")]
        public Text textName;

        public GameConfiguration gameConfiguration;

        public void Initialize()
        {
            textName.text = gameConfiguration.name;
        }
    }
}