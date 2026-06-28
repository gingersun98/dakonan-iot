using Proyecto26;
using TMPro;
using UnityEngine;
using static UnityEngine.Audio.ProcessorInstance;

public class ScalePanel : MonoBehaviour
{
	public TextMeshProUGUI scaleAmount;

	public TextMeshProUGUI creditAmount;

	public TextMeshProUGUI oldBalance;

	public TextMeshProUGUI newBalance;

	public Animator selfAnim;
	public Animator[] valueUpdate;

	public bool randomizedValue;
	[HideInInspector]
	public string storedScale;

	private int storedBalance;

	private float refreshTimer;

	private bool hasRefreshed;

	public void Initialize(string scale, int lastBalance)
	{
		randomizedValue = false;
		storedScale = scale;
		storedBalance = lastBalance;
		refreshTimer = 0;
		hasRefreshed = false;

        var request = new RequestHelper
        {
            Uri = MainMenu.Instance.baseLink + MainMenu.Instance.getScaleEndpoint + scale,
            Method = "GET",
            Headers = new System.Collections.Generic.Dictionary<string, string>
            {
                { "Authorization", "Bearer " + PlayerPrefs.GetString("token", "")}
            }
        };

        RestClient.Get<ScaleResponse>(request)
            .Then(response =>
            {
                scaleAmount.text = response.weight.ToString("N0") + "<size=20><br>GRAM";
                creditAmount.text = "<sprite index=0> " + (response.weight / 100).ToString("N0");
                oldBalance.text = "<sprite index=0> " + storedBalance.ToString("N0");
                newBalance.text = "<sprite index=0> " + (storedBalance + response.weight / 100).ToString("N0");
                foreach (Animator anim in valueUpdate)
                {
                    anim.Play("ScaleAmountUpdate", 0, 0f);
                }
            })
            .Catch(error =>
            {
                print("Error while fetching scale's information : " + error + " | scaleId : " + storedScale);
                FakeInitialize();
            });

		if (TutorialManager.Instance.IsTutorialActive() && TutorialManager.Instance.GetActiveState().progressAfterScan)
		{
			TutorialManager.Instance.StartTutorial();
		}
	}

	public void FakeInitialize()
	{
		randomizedValue = true;
		storedScale = "";
        storedBalance = 0;
        int weight = Random.Range(0, 100000);
        scaleAmount.text = weight.ToString("N0") + "<size=20><br>GRAM";
        creditAmount.text = "<sprite index=0> " + (weight / 100).ToString("N0");
        oldBalance.text = "<sprite index=0> " + storedBalance.ToString("N0");
        newBalance.text = "<sprite index=0> " + (storedBalance + weight / 100).ToString("N0");
        foreach (Animator anim in valueUpdate)
        {
            anim.Play("ScaleAmountUpdate", 0, 0f);
        }
    }

	private void OnDisable()
	{
		refreshTimer = 0;
		hasRefreshed = false;
	}

	public void Update()
	{
		refreshTimer += Time.deltaTime;
		if (refreshTimer >= 2f && !hasRefreshed && !randomizedValue && !string.IsNullOrEmpty(storedScale))
		{
			hasRefreshed = true;

            var request = new RequestHelper
            {
                Uri = MainMenu.Instance.baseLink + MainMenu.Instance.getScaleEndpoint + storedScale,
                Method = "GET",
                Headers = new System.Collections.Generic.Dictionary<string, string>
            {
                { "Authorization", "Bearer " + PlayerPrefs.GetString("token", "")}
            }
            };

            RestClient.Get<ScaleResponse>(request)
                .Then(response =>
                {
                    if (scaleAmount.text != response.weight.ToString("N0") + "<size=20><br>GRAM")
                    {
                        foreach (Animator anim in valueUpdate)
                        {
                            anim.Play("ScaleAmountUpdate", 0, 0f);
                        }
                    }
                    scaleAmount.text = response.weight.ToString("N0") + "<size=20><br>GRAM";
                    creditAmount.text = "<sprite index=0> " + (response.weight / 100).ToString("N0");
                    oldBalance.text = "<sprite index=0> " + storedBalance.ToString("N0");
                    newBalance.text = "<sprite index=0> " + (storedBalance + response.weight / 100).ToString("N0");
                    refreshTimer = 0;
                    hasRefreshed = false;
                })
                .Catch(error =>
                {
                    print("Error while fetching scale's information : " + error + " | scaleId : " + storedScale);
                    var reqEx = error as Proyecto26.RequestException;

                    if (reqEx != null && (reqEx.StatusCode == 403 || reqEx.StatusCode == 401))
                    {
                        MainMenu.Instance.StartNotification("Koneksi terputus.");
                        MainMenu.Instance.StopScaleConnection();
                    } else
                    {
                        refreshTimer = 0;
                        hasRefreshed = false;
                    }
                });

            /*
            RestClient.Get<ScaleResponse>(MainMenu.Instance.baseLink + MainMenu.Instance.getScaleEndpoint + storedScale).Then(response =>
            {
                if (scaleAmount.text != response.weight.ToString("N0") + "<size=20><br>GRAM")
                {
                    foreach (Animator anim in valueUpdate)
					{
						anim.Play("ScaleAmountUpdate", 0, 0f);
					}
                }
                scaleAmount.text = response.weight.ToString("N0") + "<size=20><br>GRAM";
                creditAmount.text = "<sprite index=0> " + (response.weight / 100).ToString("N0");
                oldBalance.text = "<sprite index=0> " + storedBalance.ToString("N0");
                newBalance.text = "<sprite index=0> " + (storedBalance + response.weight / 100).ToString("N0");
				refreshTimer = 0;
				hasRefreshed = false;
            }).Catch(error =>
            {
                print("Error while fetching scale's information : " + error + " | scaleId : " + storedScale);
				refreshTimer = 0;
				hasRefreshed = false;
            });
            */ // OLD
        } else if (randomizedValue && refreshTimer >= 0.2f)
		{
            foreach (Animator anim in valueUpdate)
            {
                anim.Play("ScaleAmountUpdate", 0, 0f);
            }
            storedBalance = 0;
            int weight = Random.Range(0, 100000);
            scaleAmount.text = weight.ToString("N0") + "<size=20><br>GRAM";
            creditAmount.text = "<sprite index=0> " + (weight / 100).ToString("N0");
            oldBalance.text = "<sprite index=0> " + storedBalance.ToString("N0");
            newBalance.text = "<sprite index=0> " + (storedBalance + weight / 100).ToString("N0");
			refreshTimer = 0;
        }
    }
}
