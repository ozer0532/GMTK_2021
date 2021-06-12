using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    //Kamus 
	public Slider slider; //Mengambil slider dari unity
	public Gradient gradient; //Mengambil gradien dari unity
    public Image fill; //Mengambil gambar "fill" dari unity
    public int currentBar;  //Besar nilai health 
    public int startingBar; //Start health
    public int maxBar = 100; //Nilai maksimum health

    //Set nilai maksimum pada slider
    public void SetMaxBar(int bar)
	{
		slider.maxValue = bar; //inisialisasi slider
		slider.value = 0;

        fill.color = gradient.Evaluate(1f); //mengubah warna gradien menjadi penuh
	}

    //Set nilai slider
    public void SetBar(int bar)
	{
		slider.value = bar; //mengubah slider

		fill.color = gradient.Evaluate(slider.normalizedValue); 
	}

    //Menambah nilai
    public void reduceBar(int bar)
    {
        currentBar += bar; //menambah nilai sebesar nilai heal
        if (currentBar<0) //health maksimum 100
            {
            currentBar = 0;
        }
        else if (currentBar > maxBar)
        {
            currentBar = maxBar;
        }
        slider.value = currentBar; //mengubah slider
        fill.color = gradient.Evaluate(slider.normalizedValue); 
        SetBar(currentBar); //mengubah nilai slider
    }

    // Start is called before the first frame update
    void Start()
    {
        SetMaxBar(100);
        currentBar = startingBar;
    }

    // Update is called once per frame
    void Update()
    {
        SetBar(currentBar);
    }
}
