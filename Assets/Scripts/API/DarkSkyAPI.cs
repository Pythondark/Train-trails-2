using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using ForecastIO;
using Newtonsoft.Json;
using TMPro;
using System.Threading.Tasks;


public class DarkSkyAPI : MonoBehaviour
{
    [Header("Location")]
    [SerializeField] float latitude  = 49.2827f;
    [SerializeField] float longitude = -123.1207f;

    //[Header("Get Settings")]
    //[SerializeField] string site = "https://api.darksky.net/";
    //[SerializeField] string nextField = "forecast";
    //[SerializeField] string exclude = "";
    //[SerializeField] string extend = "";

    //[Header("View Settings")]
    //[SerializeField] bool showLatitude = false;
    //[SerializeField] bool showLongitude = false;
    //[SerializeField] bool showTimezone = false;
    //[SerializeField] bool showTemperature = false;
    //[SerializeField] bool showSummary = false;

    private const string APIKey = "44fbd6ac0e4a139ce395559084a25b2a";

    //https://api.darksky.net/forecast/44fbd6ac0e4a139ce395559084a25b2a/37.8267,-122.4233

    public TextMeshProUGUI responseText;

    public void OnButtonRequest()
    {
        responseText.text = "Downloading data...";
        StartCoroutine(RequestWrapper());

    }

    private IEnumerator RequestWrapper()
    {
        yield return StartCoroutine(SimpleGetRequest());
    }

    private IEnumerator SimpleGetRequest()
    {
        var request = new ForecastIORequest(APIKey, latitude, longitude, Unit.ca);
        var response = request.Get();

        responseText.text = "Timezone: " + response.timezone;
        responseText.text += "\nTemperature: " + (string.Format("{0}", response.currently.apparentTemperature));
        responseText.text += "\nIcon: " + (string.Format("{0}", response.currently.icon));
        responseText.text += "\nTime: " + (string.Format("{0}", response.currently.time));

        yield return new WaitForEndOfFrame();
    }



    /*
    public async void Request()
    {
        //var request = new ForecastIORequest(APIKey, latitude, longitude, Unit.ca);
        //var response = request.Get();

        ForecastIOResponse response = (await GetWeather());


        responseText.text = "Timezone: " + response.timezone;
        responseText.text += "\nTemperature: " + (string.Format("{0}", response.currently.apparentTemperature));
        responseText.text += "\nIcon: " + (string.Format("{0}", response.currently.icon));
        responseText.text += "\nTime: " + (string.Format("{0}", response.currently.time));
    }

    private async Task<ForecastIOResponse> GetWeather()
    {
        ForecastIORequest request = new ForecastIORequest(APIKey, latitude, longitude, Unit.ca);
        ForecastIOResponse response = (ForecastIOResponse)(await request.Get());
        return response;
    }
    */
}
