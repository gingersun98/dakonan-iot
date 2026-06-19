# 🎮 TUTORIAL GAME CONGKLAK - UNITY
### Versi 1.0 - Konversi Berat ke Credit, Bermain Congklak, DLL.
---

## 🗺️ ALUR GAME
```
┌─────────────────────────┐
│       Menu Utama        │
└───────────┬─────────────┘
            │
    ┌───────┼───────────────┬───────────────┐
    │       │               │               │
    ▼       ▼               ▼               ▼
   MAIN  PENGATURAN       CREDIT          KELUAR
    │       │               │               │
    │       │               │               ▼
    │       │               │        Keluar Aplikasi
    │       │               │
    │       │               ▼
    │       │        Scan Kode QR
    │       │               │
    │       │               ▼
    │       │   Sambungkan Aplikasi ke Timbangan
    │       │               │
    │       │               ▼
    │       │    Masukan Sampah ke Timbangan
    │       │               │
    │       │               ▼
    │       │      Hitung Berat Sampah
    │       │               │
    │       │               ▼
    │       │      Setiap 100g = 1 Credit
    │       │               │
    │       │               ▼
    │       │          Tukar Credits
    │       │               │
    │       │               ▼
    │       │       Kembali ke Menu Utama
    │       │
    │       ▼
    │   Ganti Volume Musik
    │           │
    │           ▼
    │    Ganti Volume SFX
    │           │
    │           ▼
    │    Kembali ke Menu Utama
    │
    ▼
Pilih Mode Game [CONGKLAK, GASING, KETAPEL, KELERENG]
    │
    ▼
Cek Credits (Apakah Punya 5?)
    │
 ┌──┴─────┐
 │        │
 ▼        ▼
Tidak     Ya
 │        │
 ▼        ▼
Menu    Pakai
Utama  5 Credits
          │
          ▼
┌─────────────────────────┐
│    MULAI CONGKLAK       │
└─────────┬───────────────┘
 Batu Gunting Kertas
(Menentukan Giliran Pertama)
          │
          ▼
     Mulai Permainan
          │
          ▼
 Pemain Mengambil Batu
     Ke Rumah Sendiri
          │
          ▼
 Apakah semua batu sudah masuk ke rumah?
      ┌───┴───┐
      │       │
      ▼       ▼
    Tidak     Ya
      │       │
      ▼       ▼
 Lanjut     Hitung Total
 Bermain    Batu dalam Rumah
                │
                ▼
          Bandingkan Jumlah
                │
                ▼
         Pilih Pemenang
                │
                ▼
           Main Lagi?
            ┌──┴──┐
            │     │
            Ya    Tidak
            │     │
            ▼     ▼
    Mulai Match    Menu
     Dari Awal     Utama
```

---

## 📁 TUTORIAL 1 - PERSIAPAN PROJEK

### Langkah 1.1 — Buat Project Baru
1. Buka **Unity Hub → New Project**
2. Pilih template: **Universal 2D**
3. Nama: `Congklak-Dakon`
4. Klik **Create Project**

### Langkah 1.2 — Buat Struktur Folder
Klik kanan di panel **Project → Create → Folder**:
```
Assets/
├── Scripts/
├── Scenes/
├── Sprites/
└── Prefabs/
```

### Langkah 1.3 — Buat 2 Scene
**File → New Scene → Save** sebanyak 3x:

| Nama Scene | Simpan di | Tujuan |
|------------|-----------|--------|
| `MainMenu` | Assets/Scenes/ | Memilih mode game, mengkonversi kredit, dll. |
| `Gameplay` | Assets/Scenes/ | Menyimpan permainan yang akan dimainkan. |

### Langkah 1.4 — Daftarkan ke Build Settings
1. **File → Build Settings**
2. Drag semua scene ke kotak **Scenes In Build**
3. Urutan wajib:

| Index | Scene |
|-------|-------|
| 0 | MainMenu |
| 1 | Gameplay |

### Langkah 1.5 — Install Package
Sebelum memulai, pastikan semua package ini sudah terinstall kedalam projek-mu.

| Nama Package | Tempat Download | Fungsi |
|--------------|-----------------|--------|
| Easy Transitions | [Add to My Assets](https://assetstore.unity.com/packages/tools/gui/easy-transitions-225607) > Package Manager > My Assets | Mempermudah menambah animasi transisi dalam game. |
| QR Code Sharing System | [Add to My Assets](https://assetstore.unity.com/packages/tools/game-toolkits/qr-code-sharing-system-287323) > Package Manager > My Assets | Menambah sistem scan QR code yang diperlukan untuk sistem timbangan. |
| Rest Client for Unity | [Add to My Assets](https://assetstore.unity.com/packages/p/rest-client-for-unity-102501) > Package Manager > My Assets | Mempermudah mengakses backend dengan REST API. |
| Cinemachine | Package Manager > Unity Registry | Sebagai penambah dekorasi disaat pemain menang permainan. |
| StarterPack_ReplayAudio | [Download Unity Package](./ExportedPackages/StarterPack_ReplayAudio.unitypackage) > Import Package > Custom Package... | Mempercepat pembuatan sistem audio dan pencarian audio. |

---

## 🗺️ TUTORIAL 2 - PEMBUATAN TAMPILAN CONGKLAK

Buka scene "Gameplay" untuk memulai tutorial ini. Kita membuat tampilan sederhana untuk permainan `Congklak`.

### Langkah 2.1 — Buat Papan Gameplay
1. Hierarchy → Klik Kanan → **2D Object → Sprites → Square**
2. Rename: `Board`
3. **Transform:**
   - Position: `(0, 0, 0)`
   - Scale: `(1, 1, 1)`
4. **Sprite Renderer**
   - Color: `233, 175, 85, 255)` → Bebas warna apa, sesuaikan dengan selera.
   - Draw Mode : Sliced
   - Size : `(2.11, 8.25)` → Tidak harus sama persis, sesuaikan dengan ukuran layar.
   - Order In Layer : -1

### Langkah 2.2 — Buat Satu Lubang
1. Hierarchy → Klik Kanan → **2D Object → Sprites → Square**
2. Rename: `Regular Hole`
3. **Sprite Renderer**
   - Color: `(0, 120, 202, 255)` untuk warna pemain, `(156, 0, 21, 255)` untuk warna musuh. → Pilih salah satu terlebih dahulu, kita akan mengganti warnanya disaat Langkah 2.3
   - Draw Mode : Sliced
   - Size : `(0.71, 0.63)` → Tidak harus sama persis, sesuaikan dengan ukuran `Board`.
   - Order In Layer : 0
4. Tambahkan 6 komponen ini di dalam `Regular Hole` :

| Nama Komponen | Offset | Size/Radius | IsTrigger |
|---------------|--------|-------------|-----------|
| `CircleCollider2D` | (0, 0) | 0.01 | ❌ |
| `BoxCollider2D` | (-0.35, 0)* | (0.05, 0.61)* | ❌ |
| `BoxCollider2D` | (0.35, 0)* | (0.05, 0.61)* | ❌ |
| `BoxCollider2D` | (0, 0.32)* | (0.76, 0.05)* | ❌ |
| `BoxCollider2D` | (0, -0.31)* | (0.76, 0.05)* | ❌ |
| `BoxCollider2D` | (0, 0) | (0.68, 0.68) | ✅ |

*Tidak harus sama persis, asalkan bisa membentuk dinding yang menutupi `Regular Hole`

5. Buat script baru di dalam folder **Assets → Scripts** dengan nama `BallContainer.cs`.

`BallContainer.cs` berguna untuk memainkan dan menyimpan bola/batu yang akan digunakan untuk bermain congklak.
```csharp
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BallContainer : MonoBehaviour
{
    #region State

    [Header("State")]
    public bool isPlayerSide;
    public bool isSelected;

    #endregion

    #region UI

    [Header("UI")]
    public GameObject playerText;
    public GameObject enemyText;

    public TextMeshPro[] displayText;

    public GameObject selectedIndicator;

    #endregion

    #region Ball Storage

    [Header("Ball Storage")]
    List<GameObject> ballInside = new List<GameObject>();

    #endregion

    #region Animation

    [Header("Animation")]
    Coroutine jitterCoroutine;

    float maxJitterAngle = 45f;
    float recoverySpeed = 10f;

    Quaternion originalRotation;

    #endregion

    private void Start()
    {
        originalRotation = displayText[0].transform.localRotation;
    }

    private void Update()
    {
        selectedIndicator.SetActive(isSelected && isPlayerSide);
    }

    public void TriggerJitter()
    {
        // If it's already jittering, stop it so we don't break the loop
        if (jitterCoroutine != null)
        {
            StopCoroutine(jitterCoroutine);
        }

        jitterCoroutine = StartCoroutine(DoJitter());
    }

    private IEnumerator DoJitter()
    {
        // 1. Pick a random direction: left (negative) or right (positive)
        // We use a range that excludes 0 so it always snaps noticeably
        Transform whatToMove = isPlayerSide ? displayText[1].transform : displayText[0].transform;
        float randomSign = Random.value > 0.5f ? 1f : -1f;
        float randomAngle = Random.Range(maxJitterAngle * 0.5f, maxJitterAngle) * randomSign;

        // 2. Snap instantly to the jittered rotation
        // Note: For 2D / Top-down, use Vector3.forward (Z-axis). For 3D characters, use Vector3.up (Y-axis).
        Quaternion jitteredRotation = originalRotation * Quaternion.AngleAxis(randomAngle, Vector3.forward);
        whatToMove.localRotation = jitteredRotation;

        // 3. Smoothly rotate back to the original rotation
        while (Quaternion.Angle(whatToMove.localRotation, originalRotation) > 0.1f)
        {
            whatToMove.localRotation = Quaternion.Slerp(
                whatToMove.localRotation,
                originalRotation,
                Time.deltaTime * recoverySpeed
            );

            yield return null; // Wait for the next frame
        }

        // Snap precisely to the end to prevent micro-movements
        whatToMove.localRotation = originalRotation;
    }

    public void InsertBall(GameObject ball)
    {
        ball.transform.SetParent(transform, true);
        ballInside.Add(ball);
        UpdateUI();
        TriggerJitter();
    }

    public GameObject[] TakeAllBalls()
    {
        GameObject[] newArray = ballInside.ToArray();
        ballInside.Clear();
        UpdateUI();
        return newArray;
    }

    public int CheckBallCount()
    {
        return ballInside.Count;
    }

    public void OnClicked()
    {
        if (!isPlayerSide || CheckBallCount() <= 0)
            return;
        SoundManager.instance.Play("click");
        if (!isSelected)
        {
            GameManager.Instance.DeselectAll();
            isSelected = true;
        } else
        {
            StartCoroutine(GameManager.Instance.StartPlayerTurn(this));
        }
    }

    public void UpdateUI()
    {
        if (CheckBallCount() <= 0)
        {
            playerText.gameObject.SetActive(false);
            enemyText.gameObject.SetActive(false);
            return;
        }
        playerText.gameObject.SetActive(isPlayerSide);
        enemyText.gameObject.SetActive(!isPlayerSide);

        foreach(TextMeshPro text in displayText)
        {
            text.text = CheckBallCount().ToString();
        }
    }
}
```

| Nama Function | Parameters | Penjelasan |
|---------------|------------|------------|
| `DoJitter` & `TriggerJitter` | ❌ | Membuat animasi pergerakan kecil dalam teks. |
| `InsertBall` | `GameObject ball` | Memasukkan bola ke dalam lubang. |
| `TakeAllBalls` | ❌ | Mengambil semua bola yang ada di dalam lubang. |
| `CheckBallCount` | ❌ | Mengecek berapa banyak bola di dalam lubang. |
| `OnClicked` | ❌ | Mengubah state lubang menjadi `isSelected = true`, dan apabila `isSelected` sudah true, akan memulai giliran pemain untuk jalan.|
| `UpdateUI` | ❌ | Mengubah tampilan lubang. |

6. Tambahkan script ini ke dalam `Regular Hole`.
7. Buat indikator jumlah bola dalam lubang menggunakan `TextMeshPro`. Klik kanan `Regular Hole` di **Hierarchy → 2D Object → Sprites → Square**, lalu rename menjadi `Player Indicator`. Pasang di kanannya `Regular Hole`.
8. Klik kanan `Player Indicator` di **Hierarchy → Create Empty**, lalu rename menjadi `Player Display Text`. Rapikan sesuai keinginanmu.
9. Duplikat `Player Indicator`, rename menjadi `Enemy Indicator`. Pasang di kirinya `Regular Hole`.
10. Di `BallContainer.cs` dalam `Regular Hole` :
- Tarik `Player Indicator` ke variable **Player Indicator**.
- Tarik `Enemy Indicator` ke variable **Enemy Indicator**.
- Tarik kedua `Player Display Text` ke variable **Display Text**.
11. Buat indicator ketika pemain memilih lubang ini, seperti :
- Klik kanan `Regular Hole` di **Hierarchy → Create Empty**, rename menjadi `Selected Indicator`.
- Taruh di posisi (0, 0, 0).
- Klik kanan `Selected Indicator` di **Hierarchy → Create Empty**, rename menjadi `Indicator Text`.
- Tambah `TextMeshPro` di dalam `Indicator Text`. Tulis isinya dengan **"Main?"**. Ubah **Order In Layer** dalam `TextMeshPro` menjadi 2.
- Tarik `Selected Indicator` ke variable `Selected Indicator` di `BallContainer.cs`.
12. Tarik `Regular Hole` ke **Project → Prefabs** untuk menjadikan ke Prefab.
13. Hapus `Regular Hole` di Scene.
  
### Langkah 2.3 — Memasang Lubang
1. Klik kanan `Board` di **Hierarchy → Create Empty**
2. Rename : `Player Grid Holes`
3. **Transform:**
   - Position: `(0.47, 0, 0)`
   - Scale: `(1, 1, 1)`
4. Add Component → New script → `SpriteGridLayout` :

`SpriteGridLayout.cs` digunakan untuk menyusun objek dalam scene diluar `Canvas`, seperti `GridLayoutGroup` tapi untuk `SpriteRenderer`.
```csharp
using UnityEngine;

[ExecuteAlways]
public class SpriteGridLayout : MonoBehaviour
{
    public Vector2 cellSize = new Vector2(1f, 1f);
    public Vector2 spacing = new Vector2(0.1f, 0.1f);

    public int columns = 4;

    public bool centerGrid = false;

    [ContextMenu("Refresh Grid")]
    public void RefreshGrid()
    {
        int childCount = transform.childCount;

        int rows = Mathf.CeilToInt((float)childCount / columns);

        Vector2 offset = Vector2.zero;

        if (centerGrid)
        {
            float width = (columns - 1) * (cellSize.x + spacing.x);
            float height = (rows - 1) * (cellSize.y + spacing.y);

            offset = new Vector2(-width / 2f, height / 2f);
        }

        for (int i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);

            int row = i / columns;
            int col = i % columns;

            Vector3 pos = new Vector3(
                col * (cellSize.x + spacing.x),
                -row * (cellSize.y + spacing.y),
                0f
            );

            child.localPosition = pos + (Vector3)offset;
        }
    }

    private void OnValidate()
    {
        RefreshGrid();
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
            RefreshGrid();
#endif
    }
}
```
5. Jangan lupa untuk memindah file `SpriteGridLayout.cs` di folder Scripts.
6. Ambil prefab `Regular Hole` dan tarik ke objek `Player Grid Holes` di Hierarchy. Lakukan ini sebanyak 7 kali.
7. Ganti semua warna `Regular Holes` di dalam `Player Grid Holes` menjadi warna pemain, yaitu `(0, 120, 202, 255)`.
8. Nyalakan `IsPlayerSide` dalam `BallContainer.cs` di masing-masing `Regular Holes` yang ada di dalam `Player Grid Holes`.
9. Duplikat `Player Grid Holes` lalu rename ke `Enemy Grid Holes`.
10. **`Enemy Grid Holes` Transform:**
   - Position: (-0.47, 0, 0)
   - Scale: (1, 1, 1)
11. Ganti semua warna `Regular Holes` di dalam `Enemy Grid Holes` menjadi warna musuh, yaitu `(156, 0, 21, 255)`.
12. Matikan `IsPlayerSide` dalam `BallContainer.cs` di masing-masing `Regular Holes` yang ada di dalam `Enemy Grid Holes`.

### Langkah 2.4 — Membuat Rumah Pemain dan Musuh
1. Tarik prefab `Regular Hole` dari foldernya ke dalam `Board`. Rename menjadi `Player's Base`.
2. **Transform** :
   - Position: `(0, -3.38, 0)`
3. **Sprite Renderer** :
   - Color: `(0, 120, 202, 255)`
   - Size: `(1.74, 1.03)`
4. **BallContainer.cs** :
   - IsPlayerSide: `false`
   - PlayerIndicator: `null`
   - EnemyIndicator: `null`
5. Ubah `BoxCollider2D` sehingga sesuai dengan ukuran baru. Untuk `BoxCollider2D` dengan **IsTrigger** yang nyala, hapus. Karena tidak akan digunakan.

| Nama Komponen | Offset | Size | IsTrigger |
|---------------|--------|---------|-----------|
| `BoxCollider2D` | (-0.85, 0)* | (0.05, 1.03)* | ❌ |
| `BoxCollider2D` | (0.85, 0)* | (0.05, 1.03)* | ❌ |
| `BoxCollider2D` | (0, 0.5)* | (1.74, 0.05)* | ❌ |
| `BoxCollider2D` | (0, -0.5)* | (1.74, 0.05)* | ❌ |
| `BoxCollider2D` | HAPUS | HAPUS | ✅ |

*Tidak harus sama persis, asalkan bisa membentuk dinding yang menutupi `Regular Hole`
6. Duplikat `Player's Base` dan rename menjadi `Enemy's Base`.
7. **Transform** :
   - Position: `(0, 3.38, 0)`
8. **Sprite Renderer** :
   - Color: `(156, 0, 21, 255)`

### Langkah 2.5 — Membuat Inventory Pemain & Musuh
1. Klik kanan `Board` di **Hierarchy → 2D Object → Sprites → Square**. Rename menjadi `Player Inventory`.
2. **Sprite Renderer** :
   - Size: (1.47, 0.71)
3. **Transform** :
   - Position: (0, -4.69, 0)
4. Tambah komponen **Collider2D** ini :

| Nama Komponen | Offset | Size/Radius | IsTrigger |
|---------------|--------|-------------|-----------|
| `CircleCollider2D` | (0, 0) | 0.04 | ❌ |
| `BoxCollider2D` | (-0.68, 0)* | (0.08, 0.76)* | ❌ |
| `BoxCollider2D` | (0.68, 0)* | (0.08, 0.76)* | ❌ |
| `BoxCollider2D` | (0, -0.32)* | (1.42, 0.07)* | ❌ |
| `BoxCollider2D` | (0, 0.32)* | (1.42, 0.07)* | ❌ |

*Tidak harus sama persis, asalkan bisa membentuk dinding yang menutupi `Player Inventory`

5. Tambahkan objek sebagai child `Player Inventory`, dan tambahakan **TextMeshPro**. Ini akan menjadi score untuk menghitung bola di akhir game. Matikan objek ini untuk sementara.
6. Duplikat `Player Inventory`, rename menjadi `Enemy Inventory`.
7. **Transform** :
   - Position: `(0, 4.69, 0)`
  
### Langkah 2.6 — Membuat Bola
1. Klik kanan di **Hierarchy → 2D Object → Sprites → Circle**. Rename menjadi `Ball`.
2. **CircleCollider2D** :
   - IsTrigger: `false`
4. **Rigidbody2D** :
   - Linear Damping : `62.8`
5. Tambahkan script baru dengan nama `GravityToTarget.cs`.
`GravityToTarget.cs` akan digunakan untuk mengganti point gravitasi oleh suatu **Rigidbody2D**.

```csharp
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GravityToTarget : MonoBehaviour
{
    public Transform gravityCenter;
    public float gravityStrength = 10f;

    private Rigidbody2D rb;

    public Vector2 externalForce;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // =========================
    // PERGANTIAN POINT GRAVITASI
    // =========================
    public void Initialize(Transform newTarget)
    {
        gravityCenter = newTarget;

        rb.AddForce(Vector2.one * 5);
    }

    // =========================
    // SIMULASI GRAVITASI
    // =========================
    private void FixedUpdate()
    {
        if (gravityCenter == null)
            return;

        Vector3 targetPos =
            gravityCenter.position +
            GameManager.Instance.phoneOffset;

        Vector2 direction =
            (targetPos - transform.position).normalized;

        rb.AddForce(direction * gravityStrength);
    }
}
```

6. **GravityToCenter.cs** :
   - Gravity Strength : `30`
7. Simpan objek ini sebagai prefab di folder **Prefabs**.
  
## 📺 TUTORIAL 3 - PEMBUATAN USER INTERFACE
Secara simple, kita akan fokus ke Panel Akhir Game, toggle Turbo Mode, tombol Main Menu dan Restart. Untuk hal dekorasi lainnya seperti Turn Indicator, Player Corner, Enemy Corner, dan lain-lainnya bisa ditentukan oleh kalian.

### Langkah 3.1 — Membuat Button Restart dan Main Menu
Kedua tombol ini akan digunakan di dua tempat, yaitu di Normal Gameplay dan di Panel Akhir Game. Untuk langkah ini, akan di Normal Gameplay terlebih dahulu.
1. Klik kanan di **Hierarchy → UI (Canvas) → Button - TextMeshPro**. Ini akan otomatis membuat **Canvas** dan **Button** di dalamnya.
2. **Canvas** :
   - Render Mode : `Screen Space - Camera`
   - Render Camera : `Main Camera`
   - Order In Layer : `6`
3. **Canvas Scaler** :
   - UI Scale Mode : `Scale with Screen Size`
   - Reference Resolution : `(720, 1600)`
   - Match : `0.5`
4. Di dalam **Button**, ganti **Text (TMP)** menjadi teks yang sesuai dengan tujuan buttonnya, seperti "Restart".
5. Untuk fungsi buttonnya, akan kita buat di Tutorial 4. Sementara, kita buat tempatnya dulu.
6. Duplikat button, dan ganti ke "Main Menu".
7. Taruh di posisi yang diinginkan, pastikan **Anchored Preset** di **RectTransform** sesuai dengan posisinya di layar, seperti kanan atas, atau kiri bawah.

### Langkah 3.2 — Membuat Toggle "Turbo Mode"
Turbo Mode akan digunakan untuk mempercepat gameplay "Congklak".
1. Klik kanan di **Hierarchy → UI (Canvas) → Toggle**.
2. Ganti **Label** di dalam **Toggle** menjadi teks yang sesuai, seperti "Turbo Mode".
3. Rapikan teks jika perlu, dan posisikan di tempat yang diinginkan.

### Langkah 3.3 — Membuat Panel Akhir Game
Panel Akhir Game akan muncul ketika semua bola sudah masuk ke dalam masing-masing rumah. Di tutorial ini akan hanya berisi tombol dan hasil teks, tapi bisa didekorasi sesuai keinginan.
1. Klik kanan di **Hierarchy → UI (Canvas) → Panel**. Rename menjadi `Result Panel`.
2. Duplikat tombol `Main Menu` dan `Restart` dan masukkan ke dalam panel `Result Panel`.
3. Rapikan dan sesuaikan posisi tombol dengan keinginan-mu.
4. Klik kanan di **Result Panel → UI (Canvas) → Text - TextMeshPro**. Rename menjadi `Result Text`. Posisikan sesuai keinginan.

## 📜 TUTORIAL 4 - PEMBUATAN SCRIPT

### Langkah 4.1 — Setup SoundManager.cs
1. Buka folder **Assets/AudioSetup**, dan masukkan prefab AudioManager dalam scene.
2. Di dalam `AudioManager.cs`, sudah disiapkan nama audio dan clip-clipnya.
3. Jika ingin mengganti, bisa tarik **AudioClip** yang baru ke dalam prefabnya.

### Langkah 4.2 — GameManager.cs
`GameManager.cs` adalah script yang mengurus hampir semua hal dari gameplay. Seperti pergerakan bola, penghitungan score, User Interface, dan lain-lainnya.

1. Buat script baru dengan nama `GameManager.cs`, tambahkan script ke objek bebas, tapi di tutorial ini akan dimasukkan ke `Board`.
```csharp
using EasyTransition;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    public int ballsPerHole;

    [Header("Board References")]
    public BallContainer playerBase;
    public BallContainer enemyBase;

    public List<BallContainer> playerHoles;
    public List<BallContainer> enemyHoles;

    [Header("Ball Management")]
    public ObjectPooling ballPool;

    private List<GameObject> activeBalls = new();

    [Header("UI")]
    public Toggle turboToggle;

    public GameObject resultPanel;
    public TextMeshProUGUI resultText;

    public Animator turnNotifyAnim;
    public TextMeshProUGUI turnNotifyText;

    [Header("Player & Enemy UI")]
    public Animator playerCornerAnim;
    public Animator enemyCornerAnim;

    public TextMeshPro playerVictoryText;
    public TextMeshPro enemyVictoryText;

    [Header("Animation")]
    public AnimationCurve moveCurve;

    [Header("Inventory")]
    public Transform playerInventory;
    public Transform enemyInventory;

    [Header("Cameras")]
    public GameObject playerVictoryCam;
    public GameObject enemyVictoryCam;

    [Header("Effects")]
    public CinemachineImpulseSource impulseSource;
    public CinemachineImpulseSource explodeSource;

    public ParticleSystem confettiParticle;

    [Header("Managers")]
    public RockPaperScissors rpsManager;

    [Header("Transition")]
    public TransitionSettings transition;

    [Header("Runtime")]
    private bool isPlayerTurn;

    [HideInInspector]
    public Vector3 phoneOffset;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SoundManager.instance.PlayMusic("music");
        StartGame();
    }

    private void Update()
    {
    #if UNITY_ANDROID || UNITY_IOS

        var accel = Accelerometer.current;
        Vector3 value = accel != null ? accel.acceleration.ReadValue() : Vector3.zero;

        phoneOffset = new Vector3(value.x, value.y, 0f);

    #else

        var keyboard = Keyboard.current;

        float x = 0f;
        float y = 0f;

        if (keyboard != null)
        {
            if (keyboard.aKey.isPressed)
                x = -1f;
            else if (keyboard.dKey.isPressed)
                x = 1f;

            if (keyboard.sKey.isPressed)
                y = -1f;
            else if (keyboard.wKey.isPressed)
                y = 1f;
        }

        phoneOffset = new Vector3(x, y, 0f);

#endif

        if (!isPlayerTurn)
            return;

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            TrySelectContainer(Mouse.current.position.ReadValue());
        }

        if (Touchscreen.current != null)
        {
            var touch = Touchscreen.current.primaryTouch;

            if (touch.press.wasPressedThisFrame)
            {
                TrySelectContainer(touch.position.ReadValue());
            }
        }
    }

    private void TrySelectContainer(Vector2 screenPos)
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        Vector2 worldPoint = cam.ScreenToWorldPoint(screenPos);

        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

        if (hit.collider == null)
            return;

        BallContainer container =
            hit.collider.GetComponentInParent<BallContainer>();

        if (container == null)
            return;

        container.OnClicked();
    }

    public void StartGame()
    {
        ClearBalls();

        int totalBall = ballsPerHole * 7;

        for (int i = 0; i < totalBall; i++)
        {
            AddBall(playerBase);
            AddBall(enemyBase);
        }

        isPlayerTurn = false;

        resultPanel.SetActive(false);
        if (playerVictoryCam != null)
        playerVictoryCam.SetActive(false);
        if (enemyVictoryCam != null)
        enemyVictoryCam.SetActive(false);

        if (enemyCornerAnim != null)
        enemyCornerAnim.Play("CornerEnemyDisappear", 0);
        if (playerCornerAnim != null)
        playerCornerAnim.Play("CornerProfileDisappear", 0);

        playerVictoryText.gameObject.SetActive(false);
        enemyVictoryText.gameObject.SetActive(false);

        if (rpsManager != null)
        {
            rpsManager.gameObject.SetActive(true);
            rpsManager.Initiate();
        } else
        {
            SuccessRPS(true);
        }
    }

    public void SuccessRPS(bool playerWon)
    {
        isPlayerTurn = playerWon;

        GameObject[] playerBalls = playerBase.TakeAllBalls();
        GameObject[] enemyBalls = enemyBase.TakeAllBalls();

        float delayStep = turboToggle.isOn ? 0.01f : 0.05f;
        float travelTime = turboToggle.isOn ? 0.05f : 0.2f;

        for (int i = 0; i < playerBalls.Length; i++)
        {
            GameObject ball = playerBalls[i];

            BallContainer targetHole =
                playerHoles[i % playerHoles.Count];

            GravityToTarget gravity =
                ball.GetComponent<GravityToTarget>();

            MoveToTarget(
                ball.transform,
                targetHole.transform,
                travelTime,
                i * delayStep,
                () =>
                {
                    gravity.Initialize(targetHole.transform);
                    targetHole.InsertBall(ball);
                    SoundManager.instance.Play("tap");
                });
        }

        for (int i = 0; i < enemyBalls.Length; i++)
        {
            GameObject ball = enemyBalls[i];

            BallContainer targetHole =
                enemyHoles[i % enemyHoles.Count];

            GravityToTarget gravity =
                ball.GetComponent<GravityToTarget>();

            bool isLastBall = i == enemyBalls.Length - 1;

            MoveToTarget(
                ball.transform,
                targetHole.transform,
                travelTime,
                i * delayStep,
                () =>
                {
                    gravity.Initialize(targetHole.transform);
                    targetHole.InsertBall(ball);

                    if (isLastBall)
                    {
                        if (rpsManager != null)
                        rpsManager.gameObject.SetActive(false);

                        if (isPlayerTurn)
                        {
                            SwitchPlayerTurn();
                        }
                        else
                        {
                            SwitchEnemyTurn();
                        }
                    }
                });
        }
    }

    public void AddBall(BallContainer hole)
    {
        GameObject ball = ballPool.GetObject();

        ball.transform.SetParent(hole.transform);
        ball.transform.localPosition = Vector3.zero;

        GravityToTarget gravity = ball.GetComponent<GravityToTarget>();

        if (gravity != null)
        {
            gravity.Initialize(hole.transform);
        }

        hole.InsertBall(ball);

        if (!activeBalls.Contains(ball))
        {
            activeBalls.Add(ball);
        }
    }

    public void ClearBalls()
    {
        foreach (BallContainer hole in playerHoles)
        {
            GameObject[] balls = hole.TakeAllBalls();

            foreach (GameObject ball in balls)
            {
                ballPool.ReturnObject(ball);
                activeBalls.Remove(ball);
            }
        }

        foreach (BallContainer hole in enemyHoles)
        {
            GameObject[] balls = hole.TakeAllBalls();

            foreach (GameObject ball in balls)
            {
                ballPool.ReturnObject(ball);
                activeBalls.Remove(ball);
            }
        }

        foreach (GameObject ball in playerBase.TakeAllBalls())
        {
            ballPool.ReturnObject(ball);
            activeBalls.Remove(ball);
        }

        foreach (GameObject ball in enemyBase.TakeAllBalls())
        {
            ballPool.ReturnObject(ball);
            activeBalls.Remove(ball);
        }

        foreach (GameObject ball in activeBalls)
        {
            if (ball != null && ball.activeInHierarchy)
            {
                ballPool.ReturnObject(ball);
            }
        }

        activeBalls.Clear();
    }

    public IEnumerator StartPlayerTurn(BallContainer containerToTake, bool hasSpunBefore)
    {
        isPlayerTurn = false;
        DeselectAll();

        bool isPlayerSide = containerToTake.isPlayerSide;

        GameObject[] takenBalls = containerToTake.TakeAllBalls();

        activeBalls = new List<GameObject>(takenBalls);

        foreach (GameObject ball in activeBalls)
        {
            MoveToTarget(
                ball.transform,
                playerInventory,
                0.2f,
                0f,
                null
            );

            GravityToTarget g = ball.GetComponent<GravityToTarget>();
            if (g != null)
                g.Initialize(playerInventory);
        }
        SoundManager.instance.Play("pickUp");

        yield return new WaitForSeconds(turboToggle.isOn ? 0.1f : 1f);

        bool onPlayerSide = containerToTake.isPlayerSide;
        int holeIndexProgress = onPlayerSide ? playerHoles.IndexOf(containerToTake) + 1 : enemyHoles.IndexOf(containerToTake) + 1;

        bool hasSpunAround = hasSpunBefore;

        BallContainer currentTarget = null;

        int i = 0;
        float pitchProgress = 1f;

        while (i < activeBalls.Count)
        {
            GameObject ball = activeBalls[i];

            BallContainer nextContainer = null;

            if (onPlayerSide)
            {
                if (holeIndexProgress >= playerHoles.Count)
                {
                    nextContainer = playerBase;

                    onPlayerSide = false;
                    holeIndexProgress = 0;
                }
                else
                {
                    nextContainer = playerHoles[holeIndexProgress];
                    holeIndexProgress++;
                }
            }
            else
            {
                if (holeIndexProgress >= enemyHoles.Count)
                {
                    nextContainer = playerHoles[0];
                    onPlayerSide = true;
                    holeIndexProgress = 1;

                    hasSpunAround = true;
                }
                else
                {
                    nextContainer = enemyHoles[holeIndexProgress];
                    holeIndexProgress++;
                }
            }

            currentTarget = nextContainer;

            bool wasEmpty = nextContainer.CheckBallCount() == 0;

            MoveToTarget(
                ball.transform,
                nextContainer.transform,
                0.25f,
                0f,
                null
            );

            GravityToTarget g2 = ball.GetComponent<GravityToTarget>();
            if (g2 != null)
                g2.Initialize(nextContainer.transform);

            nextContainer.InsertBall(ball);

            SoundManager.instance.Play("drop").pitch = pitchProgress;

            pitchProgress += 0.1f;

            i++;

            yield return new WaitForSeconds(turboToggle.isOn ? 0.1f : 0.15f);
        }

        yield return new WaitForSeconds(turboToggle.isOn ? 0.1f : 1f);

        if (currentTarget == playerBase)
        {
            SwitchPlayerTurn();
            yield break;
        }

        if (!currentTarget.isPlayerSide)
        {
            if (currentTarget.CheckBallCount() <= 1)
            {
                SwitchEnemyTurn();
                yield break;
            } else
            {
                StartCoroutine(StartPlayerTurn(currentTarget, hasSpunBefore));
                yield break;
            }
        }

        if (currentTarget.isPlayerSide)
        {
            bool wasEmpty = currentTarget.CheckBallCount() == 1;

            if (wasEmpty && hasSpunAround)
            {
                int index = playerHoles.IndexOf(currentTarget);
                int mirroredIndex = playerHoles.Count - 1 - index;
                BallContainer opposite = enemyHoles[mirroredIndex];
                if (opposite.CheckBallCount() > 0)
                {
                    GameObject[] capturedOpposite = opposite.TakeAllBalls();
                    GameObject[] capturedSelf = currentTarget.TakeAllBalls();

                    StartCoroutine(CaptureToBase(capturedOpposite, playerBase));
                    StartCoroutine(CaptureToBase(capturedSelf, playerBase));
                }

                SoundManager.instance.Play("capture");
                if (impulseSource != null)
                impulseSource.GenerateImpulse();
                yield return new WaitForSeconds(turboToggle.isOn ? 0.1f : 1f);

                SwitchEnemyTurn();
                yield break;
            }

            if (currentTarget.CheckBallCount() > 1)
            {
                StartCoroutine(StartPlayerTurn(currentTarget, hasSpunBefore));
                yield break;
            }

            SwitchEnemyTurn();
            yield break;
        }
    }

    private void DecideEnemyHoles()
    {
        List<BallContainer> validHoles = new List<BallContainer>();

        foreach (BallContainer hole in enemyHoles)
        {
            if (hole.CheckBallCount() > 0)
                validHoles.Add(hole);
        }

        if (validHoles.Count == 0)
            return;

        float bestScore = float.MinValue;
        List<BallContainer> best = new List<BallContainer>();

        foreach (BallContainer hole in validHoles)
        {
            BallAheadResult r = CheckBallAhead(hole, hole.CheckBallCount());

            float score = 0f;

            // ✔ capture = strongest move
            if (r.canCapture)
                score += 200f;

            // ✔ extra turn (base landing)
            if (r.landedOnBase)
                score += 120f;

            // ✔ chain potential
            if (r.causesRelay)
                score += 80f;

            // ❌ dangerous empty landing without capture
            if (r.wasEmptyLanding && !r.canCapture)
                score -= 150f;

            // small preference: more balls = longer influence
            score += hole.CheckBallCount() * 2f;

            if (score > bestScore)
            {
                bestScore = score;
                best.Clear();
                best.Add(hole);
            }
            else if (Mathf.Approximately(score, bestScore))
            {
                best.Add(hole);
            }
        }

        BallContainer chosen =
            best[UnityEngine.Random.Range(0, best.Count)];

        StartCoroutine(StartEnemyTurn(chosen, false));
    }

    public IEnumerator StartEnemyTurn(BallContainer containerToTake, bool hasSpunBefore)
    {
        isPlayerTurn = false;
        DeselectAll();
        yield return new WaitForSeconds(turboToggle.isOn ? 0.1f : 0.8f);

        GameObject[] takenBalls = containerToTake.TakeAllBalls();

        activeBalls = new List<GameObject>(takenBalls);

        foreach (GameObject ball in activeBalls)
        {
            MoveToTarget(
                ball.transform,
                enemyInventory,
                0.2f,
                0f,
                null
            );

            GravityToTarget g = ball.GetComponent<GravityToTarget>();
            if (g != null)
                g.Initialize(enemyInventory);
        }
        SoundManager.instance.Play("pickUp");

        yield return new WaitForSeconds(turboToggle.isOn ? 0.5f : 1f);

        bool onEnemySide = !containerToTake.isPlayerSide;
        int holeIndexProgress = onEnemySide ? enemyHoles.IndexOf(containerToTake) + 1 : playerHoles.IndexOf(containerToTake) + 1;

        bool hasSpunAround = hasSpunBefore;

        BallContainer currentTarget = null;

        int i = 0;
        float pitchProgress = 1f;

        while (i < activeBalls.Count)
        {
            GameObject ball = activeBalls[i];

            BallContainer nextContainer = null;

            if (onEnemySide)
            {
                if (holeIndexProgress >= enemyHoles.Count)
                {
                    nextContainer = enemyBase;

                    onEnemySide = false;
                    holeIndexProgress = 0;
                }
                else
                {
                    nextContainer = enemyHoles[holeIndexProgress];
                    holeIndexProgress++;
                }
            }
            else
            {
                if (holeIndexProgress >= playerHoles.Count)
                {
                    nextContainer = enemyHoles[0];
                    onEnemySide = true;
                    holeIndexProgress = 1;

                    hasSpunAround = true;
                }
                else
                {
                    nextContainer = playerHoles[holeIndexProgress];
                    holeIndexProgress++;
                }
            }

            currentTarget = nextContainer;

            bool wasEmptyBefore = nextContainer.CheckBallCount() == 0;

            MoveToTarget(
                ball.transform,
                nextContainer.transform,
                0.25f,
                0f,
                null
            );

            GravityToTarget g2 = ball.GetComponent<GravityToTarget>();
            if (g2 != null)
                g2.Initialize(nextContainer.transform);

            nextContainer.InsertBall(ball);

            SoundManager.instance.Play("drop").pitch = pitchProgress;

            pitchProgress += 0.1f;
            i++;

            yield return new WaitForSeconds(turboToggle.isOn ? 0.1f : 0.15f);
        }

        yield return new WaitForSeconds(turboToggle.isOn ? 0.1f : 1f);

        if (currentTarget == enemyBase)
        {
            SwitchEnemyTurn();
            yield break;
        }

        if (currentTarget.isPlayerSide == false)
        {
            bool wasEmpty = currentTarget.CheckBallCount() == 1;

            int index = enemyHoles.IndexOf(currentTarget);

            int mirroredIndex = enemyHoles.Count - 1 - index;

            BallContainer opposite = playerHoles[mirroredIndex];

            if (wasEmpty && hasSpunAround && opposite.CheckBallCount() > 0)
            {
                GameObject[] capturedOpposite = opposite.TakeAllBalls();
                GameObject[] capturedSelf = currentTarget.TakeAllBalls();

                StartCoroutine(CaptureToBase(capturedOpposite, enemyBase));
                StartCoroutine(CaptureToBase(capturedSelf, enemyBase));
                SoundManager.instance.Play("capture");
                if (impulseSource != null)
                impulseSource.GenerateImpulse();
                yield return new WaitForSeconds(turboToggle.isOn ? 0.1f : 1f);
                SwitchPlayerTurn();
                yield break;
            }

            if (currentTarget.CheckBallCount() > 1)
            {
                StartCoroutine(StartEnemyTurn(currentTarget, hasSpunAround));
                yield break;
            }

            SwitchPlayerTurn();
            yield break;
        }

        else
        {
            bool wasEmpty = currentTarget.CheckBallCount() == 1;

            if (wasEmpty)
            {
                SwitchPlayerTurn();
                yield break;
            }

            if (currentTarget.CheckBallCount() > 1)
            {
                StartCoroutine(StartEnemyTurn(currentTarget, hasSpunAround));
                yield break;
            }

            SwitchPlayerTurn();
            yield break;
        }
    }

    private void SwitchPlayerTurn()
    {
        if (CheckWin())
            return;
        int ballAmount = 0;
        foreach (BallContainer ball in playerHoles)
        {
            ballAmount += ball.CheckBallCount();
        }
        if (ballAmount <= 0)
        {
            SwitchEnemyTurn();
            return;
        }
        isPlayerTurn = true;

        if (playerCornerAnim != null)
            playerCornerAnim.Play("CornerProfileAppear");
        if (enemyCornerAnim != null)
            enemyCornerAnim.Play("CornerEnemyDisappear");

        SoundManager.instance.Play("turnChange");
        StartNotify("Giliran Pemain");
    }

    private void SwitchEnemyTurn()
    {
        if (CheckWin())
            return;
        int ballAmount = 0;
        foreach (BallContainer ball in enemyHoles)
        {
            ballAmount += ball.CheckBallCount();
        }
        if (ballAmount <= 0)
        {
            SwitchPlayerTurn();
            return;
        }
        isPlayerTurn = false;

        if (enemyCornerAnim != null)
            enemyCornerAnim.Play("CornerEnemyAppear");
        if (playerCornerAnim != null)
            playerCornerAnim.Play("CornerProfileDisappear");

        SoundManager.instance.Play("turnChange");
        StartNotify("Giliran Musuh");
        DecideEnemyHoles();
    }

    public void StartNotify(string notify)
    {
        if (turnNotifyText != null)
            turnNotifyText.text = notify;

        if (turnNotifyAnim != null)
        {
            turnNotifyAnim.gameObject.SetActive(true);
            turnNotifyAnim.Play("TurnAppear", 0, 0);
        }
    }

    public bool CheckWin()
    {
        foreach (BallContainer hole in playerHoles)
        {
            if (hole.CheckBallCount() > 0)
                return false;
        }

        foreach (BallContainer hole in enemyHoles)
        {
            if (hole.CheckBallCount() > 0)
                return false;
        }

        int playerScore = playerBase.CheckBallCount();
        int enemyScore = enemyBase.CheckBallCount();

        if (playerScore > enemyScore)
        {
            StartCoroutine(CountWinningBall(true));
        }
        else if (enemyScore > playerScore)
        {
            StartCoroutine(CountWinningBall(false));
        }
        else
        {
            resultPanel.SetActive(true);
            resultText.text = "DRAW";
        }
        return true;
    }

    private IEnumerator CountWinningBall(bool isPlayer)
    {
        BallContainer winnerBase =
            isPlayer ? playerBase : enemyBase;

        TextMeshPro winnerText =
            isPlayer ? playerVictoryText : enemyVictoryText;

        GameObject winnerCam =
            isPlayer ? playerVictoryCam : enemyVictoryCam;

        if (winnerCam != null)
        winnerCam.SetActive(true);
        winnerText.text = "0";
        winnerText.gameObject.SetActive(true);

        float pitchProgress = 1f;
        float originalTime = 0.08f;

        int normalValue = winnerBase.CheckBallCount();

        winnerText.text = "0";

        GameObject[] balls = winnerBase.TakeAllBalls();

        int currentCount = 0;

        foreach (GameObject ball in balls)
        {
            ballPool.ReturnObject(ball);

            currentCount++;

            winnerText.text = currentCount.ToString();

            AudioSource source =
                SoundManager.instance.Play("drop");

            if (source != null)
            {
                source.pitch = pitchProgress;
            }

            pitchProgress += 0.02f;

            yield return new WaitForSeconds(originalTime);

            originalTime *= 0.96f;

            if (originalTime < 0.01f)
            {
                originalTime = 0.01f;
            }
        }

        if (confettiParticle != null)
        {
            confettiParticle.transform.position = winnerBase.transform.position;
            confettiParticle.gameObject.SetActive(true);
            confettiParticle.Play();
        }

        SoundManager.instance.Play("successCounting");

        if (explodeSource != null)
            explodeSource.GenerateImpulse();

        yield return new WaitForSeconds(2f);

        resultPanel.SetActive(true);
        resultText.text =
            isPlayer ? "PLAYER WON!" : "ENEMY WON!";
    }

    public BallAheadResult CheckBallAhead(BallContainer starter, int amount)
    {
        BallAheadResult result = new BallAheadResult();

        bool onPlayerSide = starter.isPlayerSide;

        int index = onPlayerSide
            ? playerHoles.IndexOf(starter)
            : enemyHoles.IndexOf(starter);

        BallContainer current = starter;

        for (int i = 0; i < amount; i++)
        {
            index++;

            if (onPlayerSide)
            {
                if (index >= playerHoles.Count)
                {
                    current = playerBase;

                    onPlayerSide = false;
                    index = 0;
                }
                else
                {
                    current = playerHoles[index];
                }
            }
            else
            {
                if (index >= enemyHoles.Count)
                {
                    current = enemyBase;

                    onPlayerSide = true;
                    index = 0;
                }
                else
                {
                    current = enemyHoles[index];
                }
            }
        }


        result.isOnPlayerSide = current.isPlayerSide;

        result.landedOnBase =
            (current == playerBase || current == enemyBase);

        result.wasEmptyLanding = current.CheckBallCount() == 0;

        result.canCapture = false;

        if (!result.landedOnBase && result.wasEmptyLanding)
        {
            if (current.isPlayerSide)
            {
                int idx = playerHoles.IndexOf(current);

                if (idx >= 0)
                {
                    int mirroredIndex = playerHoles.Count - 1 - idx;

                    if (enemyHoles[mirroredIndex].CheckBallCount() > 0)
                    {
                        result.canCapture = true;
                    }
                }
            }
            else
            {
                int idx = enemyHoles.IndexOf(current);

                if (idx >= 0)
                {
                    int mirroredIndex = enemyHoles.Count - 1 - idx;

                    if (playerHoles[mirroredIndex].CheckBallCount() > 0)
                    {
                        result.canCapture = true;
                    }
                }
            }
        }

        result.causesRelay = (!result.landedOnBase && current.CheckBallCount() > 0);


        result.amountOfBall = current.CheckBallCount();

        return result;
    }

    public void DeselectAll()
    {
        foreach (BallContainer hole in playerHoles)
            hole.isSelected = false;

        foreach (BallContainer hole in enemyHoles)
            hole.isSelected = false;
    }

    public void SceneTravel(int index)
    {
        TransitionManager.Instance().Transition(index, transition, 0);
    }

    public void MoveToTarget(Transform movedObject, Transform target, float travelTime, float delay = 0f, Action onComplete = null)
    {
        StartCoroutine(
            MoveCoroutine(
                movedObject,
                target,
                travelTime,
                delay,
                onComplete));
    }

    public void MoveToTarget(Transform movedObject, Vector3 target, float travelTime, float delay = 0f, Action onComplete = null)
    {
        StartCoroutine(
            MoveCoroutine(
                movedObject,
                target,
                travelTime,
                delay,
                onComplete));
    }

    private IEnumerator MoveCoroutine(Transform movedObject, Transform target, float travelTime, float delay, Action onComplete = null)
    {
        if (turboToggle.isOn)
        {
            delay = 0f;
            travelTime = 0.1f;
        }
        if (delay > 0f)
            yield return new WaitForSeconds(delay);

        Vector3 startPos = movedObject.position;
        Vector3 endPos = target.position;

        float elapsed = 0f;

        while (elapsed < travelTime)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(elapsed / travelTime);

            if (moveCurve != null)
                t = moveCurve.Evaluate(t);

            movedObject.position =
                Vector3.Lerp(startPos, endPos, t);

            yield return null;
        }

        movedObject.position = endPos;

        onComplete?.Invoke();
    }

    private IEnumerator MoveCoroutine(Transform movedObject, Vector3 target, float travelTime, float delay, Action onComplete = null)
    {
        if (turboToggle.isOn)
        {
            delay = 0f;
            travelTime = 0.1f;
        }
        if (delay > 0f)
            yield return new WaitForSeconds(delay);

        Vector3 startPos = movedObject.position;
        Vector3 endPos = target;

        float elapsed = 0f;

        while (elapsed < travelTime)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(elapsed / travelTime);

            if (moveCurve != null)
                t = moveCurve.Evaluate(t);

            movedObject.position =
                Vector3.Lerp(startPos, endPos, t);

            yield return null;
        }

        movedObject.position = endPos;

        onComplete?.Invoke();
    }

    public void PlaySFX(string name)
    {
        SoundManager.instance.Play(name);
    }

    private IEnumerator CaptureToBase(GameObject[] balls, BallContainer baseContainer)
    {
        foreach (GameObject b in balls)
        {
            GravityToTarget g = b.GetComponent<GravityToTarget>();

            MoveToTarget(
                b.transform,
                baseContainer.transform,
                0.25f,
                0f,
                null
            );

            if (g != null)
                g.Initialize(baseContainer.transform);

            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(turboToggle.isOn ? 0.1f : 0.3f);

        foreach (GameObject b in balls)
        {
            baseContainer.InsertBall(b);
        }
    }
}
```

| Nama Function | Parameters | Penjelasan |
|---------------|------------|------------|
| `Update` & `TrySelectContainer` | ❌ | Mendeteksi input dari pemain dan memilih lubang mana yang dipilih oleh pemain. |
| `StartGame` | ❌ | Menyiapkan game untuk dimainkan, seperti membersihkan papan, spawning bola, dan menyiapkan `RockPaperScissors.cs` |
| `SuccessRPS` | `bool playerWon` | Memulai game setelah pemain sudah melakukan **Batu Gunting Kertas** dengan lawan. Jika `playerWon` itu true, pemain akan mulai duluan, dan sebaliknya. |
| `AddBall` | `BallContainer hole` | Memunculkan bola baru dan menaruhnya di `hole`. |
| `ClearBalls` | ❌ | Menghapus semua bola yang ada di papan. |
| `StartPlayerTurn` | `BallContainer containerToTake, bool hasSpunBefore` | Menjalankan pilihan lubang pemain. Function ini akan mengambil bola dari lubang, dan mulai memindahnya satu-satu ke lubang selanjutnya, serta memikirkan apa yang harus dilakukan selanjutnya setelah bola terakhir di pindah. |
| `DecideEnemyHoles` | ❌ | Membuat pemilihan lubang secara otomatis yang digunakan oleh musuh atau AI. Musuh akan memprioritaskan lubang yang memberikan mereka keuntungan. |
| `StartEnemyTurn` | `BallContainer containerToTake, bool hasSpunBefore` | Menjalankan pilihan lubang musuh. Sama seperti `StartPlayerTurn` namun aturannya mengikuti perspektif musuh. |
| `SwitchPlayerTurn` & `SwitchEnemyTurn` | ❌ | Mengubah siapa pun yang bermain berikutnya, diantara pemain atau musuh. |
| `StartNotify` | `string notify` | Menunjukkan teks di tengah layar. Digunakan untuk menunjukkan giliran siapa selanjutnya, namun bersifat opsional di tutorial ini. |
| `CheckWin` | ❌ | Mengecek apakah semua bola sudah masuk ke rumah masing-masing, dan jika sudah, menentukan siapa bola yang paling banyak. |
| `CountWinningBall` | `bool isPlayer` | Melakukan animasi penghitungan bola, lalu menunjukkan `Panel Akhir Game` setelah selesai. Dijalankan setelah `CheckWin`. |
| `CheckBallAhead` | `BallContainer starter, int amount` | Mengecek bola akan jatuh di lubang mana apabila memilih `starter` dengan jumlah bola `amount`. Hanya digunakan untuk AI musuh di `DecideEnemyHoles`.|
| `DeselectAll` | ❌ | Menghilangkan pilihan pemain di semua lubang. |
| `SceneTravel` | `int index` | Melakukan perpindahan scene. Digunakan oleh tombol `Main Menu`. |
| `MoveToTarget` & `MoveCoroutine` | `Transform movedObject, Transform/Vector3 target, float travelTime, float delay, Action onComplete` | Memberikan animasi jalannya objek dari posisi awal ke posisi `target`. Hanya digunakan oleh perpindahan bola.|
| `PlaySFX` | `string name` | Memainkan sound effect. Biasa digunakan untuk tombol UI. |
| `CaptureToBase` | `GameObject[] balls, BallContainer baseContainer` | Digunakan untuk mengambil bola musuh dan bola pemain disaat CAPTURE. |


### Langkah 4.3 — ObjectPooling.cs
1. Buat script baru dengan nama `ObjectPooling.cs`, tambahkan script ke objek baru. Rename `GameObject` menjadi `Object Pooling - Ball`.
`ObjectPooling.cs` adalah teknik untuk menggunakan ulang GameObject daripada terus membuat (Instantiate) dan menghapus (Destroy) objek. Ini akan mengurangi beban CPU dan memori.

```csharp
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    public GameObject prefab;
    public Queue<GameObject> pool = new Queue<GameObject>();
    public int maxAvailable;
    public bool destroyInstead; // instead of enqueing, it will destroy
    int currentlyAvailable;
    public int prebuildObject = 0;
    List<GameObject> prebuilds;

    private void Start()
    {
        if (prebuildObject <= 0)
        {
            return;
        }

        prebuilds = new List<GameObject>();
        for (int i = 0; i < prebuildObject; i++)
        {
            GameObject prebuilt = GetObject();
            prebuilt.transform.SetParent(transform, false);
            prebuilds.Add(prebuilt);
        }
        foreach (GameObject obj in prebuilds)
        {
            ReturnObject(obj);
        }
    }

    public GameObject GetObject()
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.SetActive(true);
            return obj;
        } else
        {
            if (maxAvailable > 0)
            {
                if (currentlyAvailable >= maxAvailable)
                {
                    if (transform.parent.childCount > 0)
                    {
                        transform.GetChild(0).gameObject.SetActive(true);
                        return transform.GetChild(0).gameObject;
                    } else
                    {
                        print("There's nothing to use here!");
                        return null;
                    }
                }
                else
                {
                    currentlyAvailable++;
                    return Instantiate(prefab);
                }
            }
            else
            {
                currentlyAvailable++;
                return Instantiate(prefab);
            }
        }
    }

    public void ReturnObject(GameObject obj)
    {
        if (!destroyInstead)
        {
            obj.SetActive(false);
            pool.Enqueue(obj);
        } else
        {
            Destroy(obj);
        }
    }
}

```

| Nama Function | Parameters | Penjelasan |
|---------------|------------|------------|
| `Start` | ❌ | Membuat objek baru lalu disimpan. Hanya akan jalan apabila variable `prebuildObject` lebih dari 0. |
| `GetObject` | ❌ | Mengambil objek yang disimpan lalu diberikan ke pemain. Apabila tidak ada, objek akan di buat baru dengan **Instantiate**. |
| `ReturnObject` | `GameObject obj` | Mengembalikan objek yang digunakan ke dalam script, sehingga bisa digunakan lagi nanti. |

### Langkah 4.4 — Memasukkan Variables dan Functions
1. **ObjectPooling.cs** :
   - Prefab : `Ball` → Prefab `Ball` yang ada di folder **Prefabs**.
   - Prebuild Object : `96`

2. Di Inspector, masukkan variable yang kosong :
   - Balls Per Hole : `7`
   - Player Base : `Player Base (BallContainer)` → Lubang yang besar di bagian bawah layar, warna biru.
   - Enemy Base : `Enemy Base (BallContainer)` → Lubang yang besar di bagian atas layar, warna merah.
   - Player Holes : `Regular Hole` → Masukkan semua lubang biru yang ada di kanan layar.
   - Enemy Holes : `Regular Hole` → Masukkan semua lubang merah yang ada di kiri layar.
   - Ball Pool : `Object Pooling - Ball` → Objek yang menyimpan `ObjectPooling.cs` dengan prefab `Ball`.
   - Turbo Toggle : `Turbo Mode - Toggle` → Masukkan `Toggle` dengan label berisi `Turbo Mode`.
   - Result Panel : `Result Panel` → Masukkan Panel Akhir Game yang dibuat di Tutorial 3.3
   - Result Text : `Result Text` → Teks yang ada di dalam panel `Result Panel`.
   - Player Victory Text : `Display Value (TextMeshPro)` → Teks yang ada di dalam `Player Inventory`.
   - Enemy Victory Text : `Display Value (TextMeshPro)` → Teks yang ada di dalam `Enemy Inventory`.
   - Move Curve : `Preset 4` → Pilih sesuai keinginan, ini hanya mengatur kecepatan pergerakan bola.
   - Player Inventory : `Player Inventory (Transform)`
   - Enemy Inventory : `Enemy Inventory (Transform)`
   - RPS Manager : `null` → Kosongkan dulu, kita akan membuat ini setelah tutorial ini.
   - Transition : `CircleWipe (TransitionSettings)` → Gunakan salah satu template yang sudah ada dari package.
  
3. Beberapa variables yang kosong bersifat opsional karena hanya dekorasi.
4. Berikan masing-masing tombol `Main Menu` dan `Restart` fungsi dengan `GameManager.cs`. Dalam OnClick(), masukkan :
   `Main Menu` :
   - `Board` > `GameManager` > PlaySFX("button")
   - `Board` > `GameManager` > SceneTravel(0)
   `Restart` :
   - `Board` > `GameManager` > PlaySFX("button")
   - `Board` > `GameManager` > StartGame()

## ✊ TUTORIAL 5 - PEMBUATAN BATU GUNTING KERTAS - MINIGAME

### Langkah 5.1 — Membuat User Interface
1. Klik kanan di **Hierarchy → UI (Canvas) → Panel**. Rename menjadi `Rock Paper Scissors`.
2. Cari 3 sprite yang menunjukkan 3 jenis tangan, yaitu Batu, Gunting, dan Kertas.
3. Klik kanan di **`Rock Paper Scissors` → UI (Canvas) → Image**. Rename menjadi `Player's Hand`. Ini akan menjadi tangan yang akan digunakan pemain.
4. Taruh tangan di posisi yang diinginkan.
5. Duplikat `Player's Hand`, rename menjadi `Enemy's Hand`, dan taruh di posisi yang sesuai.
6. Ganti sprite dalam kedua tangan tersebut dengan salah satu tangan yang ingin digunakan, contoh Batu.
7. Buat dua tombol yang akan digunakan untuk mengganti tangan yang ingin dimainkan oleh pemain, misal merubah ke Batu ke Gunting. Rename kedua menjadi `Up Button` dan `Down Button`.
8. Pasang tombol ini berdekatan dan di area sekitar `Player's Hand`.
9. Buat satu tombol lagi yang akan digunakan untuk memulai game. Rename menjadi `Submit Button`.
10. Secara simple, segini sudah cukup untuk memulai script, namun jika ingin menambah dekorasi, bisa membuat **TextMeshPro - Text (UI)** di masing-masing tangan.

### Langkah 5.2 — Membuat "RockPaperScissors.cs"
1. Buat script baru dengan nama `RockPaperScissors.cs`, tambahkan script ke objek `Rock Paper Scissors`.
`RockPaperScissors.cs` akan mengurus semua sistem dalam permainan Batu Gunting Kertas, lalu mengirim hasilnya ke `GameManager.cs`.

```csharp
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RockPaperScissors : MonoBehaviour
{
    public enum HandType
    {
        Batu = 0,
        Gunting = 1,
        Kertas = 2
    }

    public Sprite[] handSprites;

    public Image playerRenderer;
    public Image enemyRenderer;

    public TextMeshProUGUI playerHandDisplay;
    public TextMeshProUGUI enemyHandDisplay;

    public GameObject[] disableOnSubmit;

    public Animator winResultAnim;
    public TextMeshProUGUI winResultText;

    private HandType playerHand;
    private HandType enemyHand;

    private Animator anim;
    Coroutine handMovement;
    bool hasSubmit;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Initiate()
    {
        playerHand = HandType.Batu;
        hasSubmit = false;
        if (handMovement != null) StopCoroutine(handMovement);
        handMovement = StartCoroutine(SpinningEnemyHand());
        UpdateUI();
    }

    public void ChangeHand(bool isNext)
    {
        int value = (int)playerHand;

        if (isNext) value++;
        else value--;

        if (value > 2) value = 0;
        if (value < 0) value = 2;

        playerHand = (HandType)value;
        UpdateUI();
    }

    void UpdateUI()
    {
        playerRenderer.sprite = handSprites[(int)playerHand];
        if (playerHandDisplay != null)
        playerHandDisplay.text = playerHand.ToString().ToUpper();
    }

    public void AcceptAnswer()
    {
        StartCoroutine(RPSProcess());
    }

    private IEnumerator RPSProcess()
    {
        foreach (var obj in disableOnSubmit)
            obj.SetActive(false);

        // random enemy hand animation
        hasSubmit = true;
        yield return new WaitForSeconds(1.4f);
        StopCoroutine(handMovement);

        enemyHand = (HandType)Random.Range(0, 3);

        enemyRenderer.sprite = handSprites[(int)enemyHand];
        if (enemyHandDisplay != null)
        enemyHandDisplay.text = enemyHand.ToString().ToUpper();

        yield return new WaitForSeconds(0.5f);

        bool playerWin =
            (playerHand == HandType.Batu && enemyHand == HandType.Gunting) ||
            (playerHand == HandType.Kertas && enemyHand == HandType.Batu) ||
            (playerHand == HandType.Gunting && enemyHand == HandType.Kertas);

        bool draw = playerHand == enemyHand;

        if (winResultAnim != null)
        {
            winResultAnim.gameObject.SetActive(true);
            winResultAnim.Play("TurnAppear", 0 ,0f);
        }

        if (draw)
        {
            if (winResultText != null)
            winResultText.text = "<size=42><b>Seri!</b></size><br>Ulangi lagi!";
            SoundManager.instance.Play("drawRPS");
            yield return new WaitForSeconds(1f);
            ResetRPS();
            yield break;
        }

        if (winResultText != null)
            winResultText.text = playerWin ? "<size=42><b>Kamu menang!</b></size><br>Kamu mendapat giliran pertama." : "<size=42><b>Kamu kalah!</b></size><br>Musuh mendapat giliran pertama.";
        if (playerWin) SoundManager.instance.Play("winRPS").pitch = 1f; else SoundManager.instance.Play("winRPS").pitch = 0.8f;

        yield return new WaitForSeconds(1.2f);

        GameManager.Instance.SuccessRPS(playerWin);
        if (anim != null)
        {
            anim.Play("PanelDisappear", 0, 0f);
        } else
        {
            gameObject.SetActive(false);
        }
    }

    private IEnumerator SpinningEnemyHand()
    {
        float t = 0;
        int index = 0;

        while (t < 1.5f)
        {
            t += Time.deltaTime;

            index = (index + 1) % handSprites.Length;

            enemyRenderer.sprite = handSprites[index];
            if (enemyHandDisplay != null)
            enemyHandDisplay.text = ((HandType)index).ToString().ToUpper();
            if (hasSubmit) SoundManager.instance.Play("button");

            yield return new WaitForSeconds((hasSubmit) ? 0.08f : 0.5f);
        }
    }

    void ResetRPS()
    {
        foreach (var obj in disableOnSubmit)
            obj.SetActive(true);
        hasSubmit = false;
        Initiate();
    }
}
```
| Nama Function | Parameters | Penjelasan |
|---------------|------------|------------|
| `Initiate` | ❌ | Menyiapkan permainan "Batu Gunting Kertas" agar bisa dimainkan. |
| `ChangeHand` | `bool isNext` | Mengganti tangan yang ingin digunakan oleh pemain. Digunakan pada kedua tombol di panel `Rock Paper Scissors`. |
| `AcceptAnswer` & `RPSProcess` | ❌ | Memulai permainan dengan tangan yang dipilih, lalu memilih tangan musuh secara acak. Jika salah satu menang, minigame berakhir. |
| `SpinningEnemyHand` | ❌ | Membuat animasi tangan musuh yang berganti tangan. |
| `ResetRPS` | ❌ | Mengulang kondisi minigame agar pemain bisa main lagi. Ini hanya jalan ketika pemain dan musuh seri. |

### Langkah 5.3 — Memasukkan Variable
1. **RockPaperScissors.cs** :
   - Hand Sprites : `Rock Sprite, Paper Sprite, Scissors Sprite` → Masukkan tiga sprite yang menunjukkan Batu, Gunting, dan Kertas.
   - Player Renderer : `Player's Hand`
   - Enemy Renderer : `Enemy's Hand`
   - Disable on Submit : `Up Button, Down Button, Submit Button` → Ketiga button yang kamu buat, masukkan ke dalam sini.
2. Untuk variable yang kosong, itu opsional. Bisa dimasukkan apabila ingin menambah dekorasi.
3. Coba game, dan kamu akan bisa memulai "Congklak" dengan giliran yang ditentukan oleh "Batu Gunting Kertas".

## ✊ TUTORIAL 6 - PEMBUATAN SCENE MAIN MENU
Dengan selesainya sistem gameplay dalam game ini, sekarang kita lanjut dalam pembuatan scene `Main Menu`.


### Langkah 6.1 — Membuat User Interface
