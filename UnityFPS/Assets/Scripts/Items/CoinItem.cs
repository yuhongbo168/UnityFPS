using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinItem : Item
{
    public int  coinPice = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            /*if (timer <= 0)*/
            //{
                Character character = collision.gameObject.GetComponent<Character>();
                if (character != null)
                {
                    CoinCanvas coinCanvas = new CoinCanvas();
                    character.SetStorePice(coinPice);
                    character.OnTakeCoinUI.Invoke(coinCanvas);


                }

                Destroy(this.gameObject);
           // }

        }
    }
    

}
