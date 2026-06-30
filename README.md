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

## 🗺️ ALUR BACKEND
```
┌──────────────────────────────┐
│       Mulai Server           │
└──────────────┬───────────────┘
               │
               ▼
 Memuat Konfigurasi & Library
 (Express, MongoDB, Firebase)
               │
               ▼
 Terhubung ke Database
               │
               ▼
 Mendaftarkan Endpoint API
               │
               ▼
     Server Siap Digunakan
          (Port 3000)
               │
     ┌─────────┼─────────────┬─────────────┬─────────────┐
     │         │             │             │             │
     ▼         ▼             ▼             ▼             ▼
 Registrasi  Login   Login Firebase   Data Timbangan   Profil
     │         │             │             │             │
     ▼         ▼             ▼             ▼             ▼
Buat Akun  Verifikasi   Verifikasi     Perbarui /   Verifikasi
Hash Sandi Kredensial Firebase Token  Lihat Berat      JWT
Buat JWT   Buat JWT          │             │             │
     │         │             ▼             ▼             ▼
     └────┬────┘     Buat Akun Jika   Simpan Berat  Tampilkan
          │          Belum Ada                     Profil & Kredit
          │                │
          └──────────► Buat JWT ◄───────────────┐
                           │                    │
                           ▼                    │
                 Pengguna Terautentikasi        │
                           │                    │
                  ┌────────┴─────────┐          │
                  │                  │          │
                  ▼                  ▼          │
              Deposit            Pembayaran     │
                  │                  │          │
                  ▼                  ▼          │
           Verifikasi JWT     Verifikasi JWT    │
                  │                  │          │
                  ▼                  ▼          │
         Periksa Timbangan   Periksa Kredit     │
                  │                  │          │
          ┌───────┴───────┐    ┌─────┴─────┐    │
          │               │    │           │    │
          ▼               ▼    ▼           ▼    │
   Tidak Valid /    Berat ≥100g?  Kredit <5  Kredit ≥5
 Sudah Diproses         │             │           │
          │        ┌────┴────┐        ▼           ▼
          ▼        │         │   Gagal Bayar  Kurangi
   Tampilkan Error ▼         ▼                5 Kredit
               Belum      Tambah Kredit          │
             Memenuhi     Tandai Deposit         ▼
             Syarat            │          Tampilkan
                               └────────► Sisa Kredit
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
| FirebaseAppCheck & FirebaseAuth | Cek Langkah 1.6 untuk Download | Membantu pembuatan sistem Account dengan Google. |

### Langkah 1.6 — Setup Firebase
**Firebase** diperlukan untuk pembuatan sistem akun dalam game, jadi pastikan sudah mempunyai projek di dalam **Firebase**.

1. Buat projek baru di **Firebase Console**

   a. Masuk ke [Firebase Console](console.firebase.google.com) untuk mulai.
   
   b. Tekan **Create a Project**
   
   c. Masukkan nama projek, pilih opsi yang sesuai dengan projek-mu, lalu klik **Create Project**.
2. Tambahkan Aplikasi Unity di Console
   
   a. Masuk ke projek yang barusan dibuat.
   
   b. Klik **Add App** lalu pilih **Unity**
   
   c. Masukkan **Bundle ID/Package Name** yang sama dengan projek Unity-mu, contoh **com.developer.replay**.
   
   d. Klik **Register App**.
3. Download SDK dari Firebase
   
   a. Disaat penambahan app, mereka juga akan memberi file SDK.
   
   b. Apabila tidak ada, bisa pergi ke [Firebase Website](https://firebase.google.com/download/unity) untuk mendownloadnya.
   
   c. Setelah download, ekstrak ke dalam folder.
   
   d. Di Unity, klik kanan di **Assets → Import Package → Custom Packages...** lalu pilih **FirebaseAppCheck** dan **FirebaseAuth**.
4. Mengaktifkan **Firebase Authentication**
   
   a. Masuk ke [Firebase Console](console.firebase.google.com), lalu ke projek-mu.
   
   b. Di bagian kiri, pilih **Security**, lalu **Authentication**.
   
   c. Pilih **Sign-in Method**, lalu tekan **Add new provider**.
   
   d. Masukkan **WebClientID** dan **SHA-1 Fingerprint** untuk menyelesaikan aktivasi.
   
   → Jika tidak memiliki **SHA-1 Fingerprint**, bisa mengikuti [tutorial](https://developers.google.com/android/guides/client-auth?sjid=16907434381778061173-NC) ini.
   
   → Jika tidak memiliki WebClientID, bisa ke **Settings > General > View in Google Cloud**.
   
   Lalu ke **APIs and services >** di tab kiri **Credentials > Web client (auto created by Google Service) > Client ID**, contohnya seperti `123456789012-akjsgaspofaklsdamn12i3g.apps.googleusercontent.com`
   
6. Download File Konfigurasi Firebase
    
   a. Pastikan lakukan ini setelah mengaktifkan **Firebase Authentication** agar file terupdate dengan **WebClientID** kita.
   
   a. Pergi ke **Settings > General > Your apps > "google-services.json".
   
   b. File ini akan kita butuhkan untuk melakukan pembuatan akun.
   
   c. Masukkan file ini ke dalam unity di **Assets** dengan klik kanan lalu **Import New Asset...** atau tarik filenya ke dalam folder.

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

### Langkah 4.2 — Setup TransitionManager.cs
1. Klik kanan di **Hierarchy → Create Empty**. Rename menjadi `Transition Manager`.
2. Di Inspector, **Add Component** → ketik **Transition Manager** → Masukkan **Transition Manager** ke dalam objek.

### Langkah 4.3 — GameManager.cs
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


### Langkah 4.4 — ObjectPooling.cs
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
   - `Board` > `GameManager` > `**PlaySFX("button")**`
   - `Board` > `GameManager` > `**SceneTravel(0)**`
     
   `Restart` :
   - `Board` > `GameManager` > `**PlaySFX("button")**`
   - `Board` > `GameManager` > `**StartGame()**`

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

## ✊ TUTORIAL 6 - PERSIAPAN PEMBUATAN SCENE "MAIN MENU"
Dengan selesainya sistem gameplay dalam game ini, sekarang kita lanjut dalam pembuatan scene `Main Menu`.

### Langkah 6.1 — Planning Pembuatan
Target kita dalam tutorial ini adalah :
- **Main Screen** → Berisi tombol Play, Settings, Credit, Exit, serta details tentang akun yang kita pakai.
- **Account Panels** → Berisi tombol untuk Sign In dengan Google, serta juga Sign In secara Guest, namun ini opsional karena hanya untuk Editor. Jika ingin Sign In secara Guest, kita juga membuat **Register** dan **Login** Tab.
- **Quit Panel** → Berisi konfirmasi untuk keluar dari game.
- **Settings Panel** → Berisi dua **Slider** yang digunakan untuk mengkontrol volume, serta tombol Back untuk menutupnya.
- **Choose Credit Panel** → Berisi tiga tombol, yaitu `Otomatis`, `Manual`, dan `Kembali`.
- **QR Scanning Panel** → Berisi **Raw Image** untuk menunjukkan kamera pemain sehingga bisa scan kode QR. Kode QR ini akan membuka **Credit Conversion Panel**.
- **Conversion Credit Panel** → Berisi teks yang menunjukkan berapa banyak gram yang ada dalam timbangan, serta berapa banyak hasil yang didapatkan. Lalu ada dua tombol untuk menerima Credit atau menutup tab.
- **Select Games Panel** → Berisi 4 tombol yang digunakan untuk memilih mode permainan apa yang ingin dimainkan. Jika menekan salah satu tombol, akan membuka **Select Games Sub-Panel**.
- **Select Games Sub-Panel (4)** → Berisi informasi mengenai mode permainan yang di tekan di **Select Games Panel**. Panel ini ada 4, dikarenakan ada 4 mode permainan yang bisa dipilih. Disini, pemain juga bisa melihat jumlah Credit serta tombol untuk memulai permainan.
- **Highlight, Tutorial Tab** → Kedua ini memiliki tujuan yang sama, yaitu sebagai petunjuk tutorial.
- **Loading Blocker** →  Panel yang menutup seluruh layar, dan berada di paling depan dari semua UI. Bertujuan untuk menunjukkan situasi ketika pemain sedang mencoba koneksi ke backend/internet.

### Langkah 6.2 — Pembuatan Script
Scene `Main Menu` memperlukan beberapa script untuk bisa jalan. Berikut adalah yang diperlukan :

1. `MainMenu.cs`
```csharp
using EasyTransition;
using Firebase.Auth;
using Google;
using Proyecto26;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public static MainMenu Instance { get; private set; }

    #region UI References

    [Header("Account UI")]
    public GameObject accountDetail;
    public TextMeshProUGUI accountName;
    public TextMeshProUGUI[] everyCurrencyShowcase;

    [Header("Authentication Panels")]
    public Animator registerTab;
    public Animator loginTab;
    public Animator signInPanel;

    [Header("Authentication Inputs")]
    public TMP_InputField registerUsernameField;
    public TMP_InputField registerPasswordField;
    public TMP_InputField loginUsernameField;
    public TMP_InputField loginPasswordField;

    [Header("Password Visibility")]
    public Image registerPasswordShow;
    public Image loginPasswordShow;
    public Sprite showPassword;
    public Sprite hidePassword;

    [Header("Notifications & Popups")]
    public TextMeshProUGUI[] notificationQRText;
    public Animator[] notificationQRAppear;
    public Animator cameraQRMenu;
    public Animator autoOrManualPanel;
    public ScalePanel modernScalePanel;

    [Header("Loading & Transitions")]
    public GameObject loadingBlocker;
    public TransitionSettings transitionSettings;

    #endregion

    #region Runtime Data

    [Header("Runtime Data")]
    [HideInInspector] public int storedBalance;

    #endregion

    #region API Configuration

    [Header("Base URL")]
    public string baseLink = "https://scaleweight-to-unity-production.up.railway.app";

    [Header("Authentication Endpoints")]
    public string registerEndpoint = "/register";
    public string loginEndpoint = "/login";
    public string firebaseEndpoint = "/firebase-login";

    [Header("User Endpoints")]
    public string profileEndpoint = "/profile";

    [Header("Scale Endpoints")]
    public string getScaleEndpoint = "/scale/";
    public string releaseScaleEndpoint = "/scale/{id}/release";
    public string autoScaleID = "timbanganID1";

    [Header("Payment Endpoints")]
    public string depositEndpoint = "/deposit";
    public string paymentEndpoint = "/payment";

    #endregion

    private void Awake()
    {
        Instance = this;
    }


    private IEnumerator Start()
    {
        SoundManager.instance.PlayMusic("music");
        while (
            FirebaseManager.Instance == null ||
            !FirebaseManager.Instance.IsReady)
        {
            yield return null;
        }

        string token =
            PlayerPrefs.GetString("token", "");

        if (string.IsNullOrEmpty(token))
        {
            signInPanel.gameObject.SetActive(true);
        }
        else
        {
            GetProfile();
        }
    }

    public void PlayGame(int gameMode)
    {
        string token = PlayerPrefs.GetString("token", "");
        loadingBlocker.SetActive(true);
        var request = new RequestHelper
        {
            Uri = baseLink + "/payment",
            Method = "POST",
            Headers = new Dictionary<string, string>
        {
            { "Authorization", "Bearer " + token }
        }
        };

        RestClient.Request<PaymentResponse>(request)
            .Then(response =>
            {
                Debug.Log("Success: " + response.success);
                Debug.Log("Remaining: " + response.remaining);
                UpdateUI(null, response.remaining);
                loadingBlocker.SetActive(false);
                switch (gameMode)
                {
                    case 0: TravelScene(1); break;
                    default: print("Not implemented yet, stay tuned!"); break;
                }
            })
            .Catch(error =>
            {
                loadingBlocker.SetActive(false);
                Debug.LogError(error);
            });
    }

    public void TravelScene(int index)
    {
        TransitionManager.Instance().Transition(index, transitionSettings, 0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Register()
    {
        string username = registerUsernameField.text.Trim();
        string password = registerPasswordField.text.Trim();
        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
        {
            loadingBlocker.SetActive(true);
            RestClient.Post<LoginResponse>(baseLink + registerEndpoint, new LoginRequest
            {
                username = username,
                password = password
            }).Then(response =>
            {
                loadingBlocker.SetActive(false);
                if (registerTab.gameObject.activeSelf)
                registerTab.Play("PanelDisappear", 0, 0f);
                if (loginTab.gameObject.activeSelf)
                loginTab.Play("PanelDisappear", 0, 0f);
                if (signInPanel.gameObject.activeSelf)
                signInPanel.Play("PanelDisappear", 0, 0f);

                PlayerPrefs.SetString("token", response.token);
                PlayerPrefs.SetInt("HasMadeAccount", 1);
                PlayerPrefs.Save();
                print("Succesful registered an account : " + username);
                accountDetail.SetActive(true);
                accountName.text = username;
                UpdateUI(username, 0);
            }).Catch(error =>
            {
                loadingBlocker.SetActive(false);
                Debug.LogError("Failure in registering a new account : " + error);
            });
        }
    }

    public void Login()
    {
        string username = loginUsernameField.text.Trim();

        string password = loginPasswordField.text.Trim();

        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
        {
            loadingBlocker.SetActive(true);

            RestClient.Post<LoginResponse>(baseLink + loginEndpoint,

                new LoginRequest
                {
                    username = username,
                    password = password
                }

            ).Then(response =>
            {
                loadingBlocker.SetActive(false);
                if (registerTab.gameObject.activeSelf)
                    registerTab.Play("PanelDisappear", 0, 0f);
                if (loginTab.gameObject.activeSelf)
                    loginTab.Play("PanelDisappear", 0, 0f);
                if (signInPanel.gameObject.activeSelf)
                    signInPanel.Play("PanelDisappear", 0, 0f);

                Debug.Log("Successfully logged in with Token: " + response.token);
                PlayerPrefs.SetInt("HasMadeAccount", 1);
                PlayerPrefs.SetString("token", response.token);
                PlayerPrefs.Save();
                accountDetail.SetActive(true);
                accountName.text = username;
                GetProfile();

            }).Catch(error =>
            {
                loadingBlocker.SetActive(false);

                Debug.LogError("Failure to log in : " + error);
            });
        }
    }

    public void LogOut()
    {
        PlayerPrefs.DeleteKey("token");
        PlayerPrefs.Save();
        ClearAllInputs();
        signInPanel.gameObject.SetActive(true);
        accountDetail.gameObject.SetActive(false);
    }
    public void ClearAllInputs()
    {
        loginPasswordField.text = "";
        loginUsernameField.text = "";
        registerPasswordField.text = "";
        registerUsernameField.text = "";
        loginPasswordField.contentType = TMP_InputField.ContentType.Password;
        registerPasswordField.contentType = TMP_InputField.ContentType.Password;
        loginPasswordField.ForceLabelUpdate();
        registerPasswordField.ForceLabelUpdate();
    }

    public void GetProfile()
    {
        string token = PlayerPrefs.GetString("token", "");

        if (string.IsNullOrEmpty(token))
        {
            Debug.Log("No token found");
            return;
        }
        loadingBlocker.SetActive(true);

        var request = new RequestHelper
        {
            Uri = baseLink + profileEndpoint,
            Method = "GET",
            Headers = new System.Collections.Generic.Dictionary<string, string>
            {
                { "Authorization", "Bearer " + token }
            }
        };

        RestClient.Get<TokenResponse>(request)
            .Then(response =>
            {
                loadingBlocker.SetActive(false);

                Debug.Log("Username: " + response.username);
                Debug.Log("Credit: " + response.credit);

                accountDetail.SetActive(true);
                accountName.text = response.username;
                UpdateUI(response.username, response.credit);
                storedBalance = response.credit;
            })
            .Catch(error =>
            {
                loadingBlocker.SetActive(false);

                Debug.LogError("Profile error: " + error);

                // IMPORTANT: token invalid or expired
                PlayerPrefs.DeleteKey("token");
                accountDetail.SetActive(false);
                UpdateUI(null, 0);
            });
    }

    public void ShowPassword()
    {
        if (registerTab.gameObject.activeSelf)
        {
            if (registerPasswordField.contentType == TMP_InputField.ContentType.Password)
            {
                registerPasswordField.contentType = TMP_InputField.ContentType.Standard;
                registerPasswordShow.sprite = showPassword;
            }
            else
            {
                registerPasswordField.contentType = TMP_InputField.ContentType.Password;
                registerPasswordShow.sprite = hidePassword;
            }
            registerPasswordField.ForceLabelUpdate();
        }
        else
        {
            if (loginPasswordField.contentType == TMP_InputField.ContentType.Password)
            {
                loginPasswordField.contentType = TMP_InputField.ContentType.Standard;
                loginPasswordShow.sprite = showPassword;
            }
            else
            {
                loginPasswordField.contentType = TMP_InputField.ContentType.Password;
                loginPasswordShow.sprite = hidePassword;
            }
           loginPasswordField.ForceLabelUpdate();
        }
    }

    public void CheckScale(string scaleID)
    {
        loadingBlocker.SetActive(true);

        var request = new RequestHelper
        {
            Uri = baseLink + getScaleEndpoint + scaleID,
            Method = "GET",
            Headers = new System.Collections.Generic.Dictionary<string, string>
            {
                { "Authorization", "Bearer " + PlayerPrefs.GetString("token", "")}
            }
        };

        RestClient.Get<ScaleResponse>(request).Then(response =>
        {
            loadingBlocker.SetActive(false);
            if (response.deposited)
            {
                Debug.LogError("Failure to get scale with ID \"" + scaleID + "\" : This scale is already deposited! Take off the weight first!");
                modernScalePanel.storedScale = "";
                for (int i =0; i < notificationQRAppear.Length; i++)
                {
                    notificationQRText[i].text = "Lepas semua berat di timbangan terlebih dahulu!";
                    notificationQRAppear[i].gameObject.SetActive(true);
                    notificationQRAppear[i].Play("TurnAppear", 0, 0f);
                }
                return;
            }
            cameraQRMenu.Play("PanelDisappear", 0, 0f);
            autoOrManualPanel.Play("PanelDisappear", 0, 0f);
            modernScalePanel.gameObject.SetActive(true);
            modernScalePanel.Initialize(scaleID, storedBalance);
        }).Catch(error =>
        {
            loadingBlocker.SetActive(false);
            Debug.LogError("Failure to get scale with ID \"" + scaleID + "\" : " + error);
            modernScalePanel.storedScale = "";
            var reqEx = error as Proyecto26.RequestException;

            if (reqEx != null)
            {
                var err =
                    JsonUtility.FromJson<ErrorMessage>(
                        reqEx.Response
                    );

                for (int i = 0; i < notificationQRAppear.Length; i++)
                {
                    notificationQRText[i].text = "" + err.message;
                    notificationQRAppear[i].gameObject.SetActive(true);
                    notificationQRAppear[i].Play("TurnAppear", 0, 0f);
                }
            }
            QRScanning.Instance.Initialize();
        });
    }

    public void AutomaticScale()
    {
        CheckScale(autoScaleID);
    }

    public void DepositScale()
    {
        if (modernScalePanel.randomizedValue)
        {
            StopScaleConnection();
            return;
        }
        if (string.IsNullOrEmpty(modernScalePanel.storedScale) && !modernScalePanel.randomizedValue)
            return;
        loadingBlocker.SetActive(true);
        string token = PlayerPrefs.GetString("token");

        RestClient.Request<DepositResponse>(
            new RequestHelper
            {
                Uri = baseLink + depositEndpoint,
                Method = "POST",
                Body = new DepositRequest
                {
                    scaleId = modernScalePanel.storedScale
                },
                Headers = new System.Collections.Generic.Dictionary<string, string>
                {
            {
                "Authorization",
                "Bearer " + PlayerPrefs.GetString("token")
            }
                }
            }
        )
        .Then(response =>
        {
            UpdateUI(null, response.totalCredit);
            storedBalance = response.totalCredit;

            StopScaleConnection();
            loadingBlocker.SetActive(false);
        })
        .Catch(error =>
        {
            Debug.LogError(error);
            loadingBlocker.SetActive(false);
            var reqEx = error as Proyecto26.RequestException;

            if (reqEx != null)
            {
                var err =
                    JsonUtility.FromJson<ErrorMessage>(
                        reqEx.Response
                    );

                for (int i = 0; i < notificationQRAppear.Length; i++)
                {
                    notificationQRText[i].text = "" + err.message;
                    notificationQRAppear[i].gameObject.SetActive(true);
                    notificationQRAppear[i].Play("TurnAppear", 0, 0f);
                }
            }
        });
    }

    public void StartNotification(string notify)
    {
        for (int i = 0; i < notificationQRAppear.Length; i++)
        {
            notificationQRText[i].text = notify;
            notificationQRAppear[i].gameObject.SetActive(true);
            notificationQRAppear[i].Play("TurnAppear", 0, 0f);
        }
    }

    public void StopScaleConnection()
    {
        if (!string.IsNullOrEmpty(modernScalePanel.storedScale))
        {
            var request = new RequestHelper
            {
                Uri = baseLink + releaseScaleEndpoint.Replace("{id}", modernScalePanel.storedScale),
                Method = "GET",
                Headers = new System.Collections.Generic.Dictionary<string, string>
            {
                { "Authorization", "Bearer " + PlayerPrefs.GetString("token", "")}
            }
            };

            RestClient.Post(request).Catch(error =>
            {
                Debug.LogError(error);
            });

            modernScalePanel.storedScale = "";
            modernScalePanel.selfAnim.Play("PanelDisappear", 0, 0f);
            autoOrManualPanel.gameObject.SetActive(true);
        }
        if (modernScalePanel.randomizedValue)
        {
            modernScalePanel.storedScale = "";
            modernScalePanel.selfAnim.Play("PanelDisappear", 0, 0f);
            autoOrManualPanel.gameObject.SetActive(true);
        }
    }

    public void SignInGoogle()
    {
        if (!FirebaseManager.Instance.IsReady)
        {
            Debug.LogError("Firebase not ready");
            return;
        }
        if (Application.platform != RuntimePlatform.Android)
        {
            Debug.Log("Google Sign-In only works on Android.");
            if (PlayerPrefs.GetInt("HasMadeAccount", 0) == 0)
            {
                registerTab.gameObject.SetActive(true);
            }
            else
            {
                loginTab.gameObject.SetActive(true);
            }
            signInPanel.Play("PanelDisappear", 0, 0f);
            return;
        }
        Debug.Log("1. SignInGoogle called");
        loadingBlocker.SetActive(true);
        GoogleSignIn.DefaultInstance
            .SignIn()
            .ContinueWith(OnGoogleLogin);
    }

    void OnGoogleLogin(Task<GoogleSignInUser> task)
    {
        Debug.Log("2. OnGoogleLogin reached");

        if (task.IsFaulted)
        {
            Debug.LogError("Google Sign-In FAILED");
            Debug.LogException(task.Exception);
            loadingBlocker.SetActive(false);
            return;
        }

        if (task.IsCanceled)
        {
            Debug.Log("Google Sign-In CANCELLED");
            loadingBlocker.SetActive(false);
            return;
        }

        if (task.Result == null)
        {
            Debug.LogError("GoogleSignInUser is NULL");
            loadingBlocker.SetActive(false);
            return;
        }

        Debug.Log("Google Sign-In SUCCESS");

        string idToken = task.Result.IdToken;

        if (string.IsNullOrEmpty(idToken))
        {
            Debug.LogError("ID TOKEN IS NULL");
            loadingBlocker.SetActive(false);
            return;
        }

        Debug.Log("ID TOKEN OK");

        var credential =
            GoogleAuthProvider.GetCredential(
                idToken,
                null
            );

        FirebaseManager.Instance.Auth
            .SignInWithCredentialAsync(credential)
            .ContinueWith(OnFirebaseAuth);
    }

    void OnFirebaseAuth(Task<FirebaseUser> task)
    {
        Debug.Log("3. OnFirebaseAuth reached");

        if (task.IsFaulted)
        {
            Debug.LogError("Firebase Auth FAILED");
            Debug.LogException(task.Exception);
            loadingBlocker.SetActive(false);
            return;
        }

        if (task.IsCanceled)
        {
            Debug.Log("Firebase Auth CANCELLED");
            loadingBlocker.SetActive(false);
            return;
        }

        FirebaseUser user = task.Result;

        if (user == null)
        {
            Debug.LogError("FirebaseUser NULL");
            loadingBlocker.SetActive(false);
            return;
        }

        Debug.Log("Firebase Auth SUCCESS");
        Debug.Log("UID: " + user.UserId);
        Debug.Log("Email: " + user.Email);

        user.TokenAsync(false)
            .ContinueWith(OnFirebaseToken);
    }

    void OnFirebaseToken(Task<string> task)
    {
        Debug.Log("4. OnFirebaseToken reached");

        if (task.IsFaulted)
        {
            Debug.LogError("Token FAILED");
            Debug.LogException(task.Exception);
            loadingBlocker.SetActive(false);
            return;
        }

        string firebaseToken = task.Result;

        Debug.Log("TOKEN RECEIVED: " + firebaseToken.Substring(0, 20) + "...");

        RestClient.Post<LoginResponse>(
            baseLink + firebaseEndpoint,
            new FirebaseLoginRequest
            {
                firebaseToken = firebaseToken
            })
        .Then(response =>
        {
            Debug.Log("5. BACKEND LOGIN SUCCESS");

            PlayerPrefs.SetString("token", response.token);
            PlayerPrefs.Save();

            signInPanel.Play("PanelDisappear", 0, 0f);

            GetProfile();
            loadingBlocker.SetActive(false);
        })
        .Catch(error =>
        {
            loadingBlocker.SetActive(false);
            Debug.LogError("BACKEND ERROR: " + error);
        });
    }

    public void UpdateUI(string username = null, int credit = 0)
    {
        if (username != null)
        {
            accountName.text = username;
        }
        foreach (TextMeshProUGUI wallet in everyCurrencyShowcase)
        {
            wallet.text = "<sprite index=0> " + credit.ToString("N0");
        }
    }

    public void PlaySFX(string sfx)
    {
        SoundManager.instance.Play(sfx);
    }
}

[System.Serializable]
public class LoginRequest
{
    public string username;
    public string password;
}

[System.Serializable]
public class LoginResponse
{
    public string token;
}

[System.Serializable]
public class TokenResponse
{
    public bool valid;
    public string username;
    public int credit;
}

[System.Serializable]
public class ScaleResponse
{
    public string scale;
    public int weight;
    public bool deposited;
}

[System.Serializable]
public class DepositRequest
{
    public string scaleId;
}

[System.Serializable]
public class DepositResponse
{
    public int earned;
    public int totalCredit;
}

[System.Serializable]
public class FirebaseLoginRequest
{
    public string firebaseToken;
}

[System.Serializable]
public class PaymentResponse
{
    public bool success;

    public int remaining;
}

[System.Serializable]
public class ErrorMessage
{
    public string message;
}
```

| Nama Function         | Parameters                                 | Penjelasan                                                                                                                             |
| --------------------- | ------------------------------------------ | -------------------------------------------------------------------------------------------------------------------------------------- |
| `Awake`               | ❌                                          | Menginisialisasi singleton `MainMenu` dengan mengisi `Instance`.                                                                       |
| `Start`               | ❌                                          | Memutar musik, menunggu Firebase siap, lalu memeriksa token login dan menampilkan panel login atau mengambil profil pengguna.          |
| `PlayGame`            | `int gameMode`                             | Mengirim request pembayaran sebelum bermain, mengurangi saldo pengguna, memperbarui UI, lalu berpindah ke mode permainan yang dipilih. |
| `TravelScene`         | `int index`                                | Berpindah ke scene tertentu menggunakan sistem transisi.                                                                               |
| `QuitGame`            | ❌                                          | Menutup aplikasi permainan.                                                                                                            |
| `Register`            | ❌                                          | Mendaftarkan akun baru menggunakan username dan password yang diinput pengguna.                                                        |
| `Login`               | ❌                                          | Melakukan login menggunakan username dan password, lalu menyimpan token dan mengambil data profil.                                     |
| `LogOut`              | ❌                                          | Menghapus token login, membersihkan input, dan menampilkan kembali panel login.                                                        |
| `ClearAllInputs`      | ❌                                          | Mengosongkan semua field login/register dan mengembalikan mode password menjadi tersembunyi.                                           |
| `GetProfile`          | ❌                                          | Mengambil data profil pengguna dari server berdasarkan token yang tersimpan.                                                           |
| `ShowPassword`        | ❌                                          | Menampilkan atau menyembunyikan password pada form login atau register.                                                                |
| `CheckScale`          | `string scaleID`                           | Memeriksa status timbangan berdasarkan ID QR yang dipindai dan membuka panel timbangan jika valid.                                     |
| `AutomaticScale`      | ❌                                          | Menyambungkan koneksi ke timbangan tertentu tanpa menggunakan kode QR.                                                                 |
| `DepositScale`        | ❌                                          | Mengirim data timbangan ke server untuk melakukan deposit dan menambahkan saldo pengguna.                                              |
| `StopScaleConnection` | ❌                                          | Menghentikan koneksi timbangan, menutup panel timbangan, dan kembali ke menu kamera QR.                                                |
| `SignInGoogle`        | ❌                                          | Memulai proses login menggunakan akun Google dan Firebase Authentication.                                                              |
| `OnGoogleLogin`       | `Task<GoogleSignInUser> task`              | Menangani hasil login Google dan melanjutkan autentikasi ke Firebase.                                                                  |
| `OnFirebaseAuth`      | `Task<FirebaseUser> task`                  | Menangani hasil autentikasi Firebase dan meminta Firebase Token.                                                                       |
| `OnFirebaseToken`     | `Task<string> task`                        | Mengirim Firebase Token ke backend untuk mendapatkan token login aplikasi.                                                             |
| `UpdateUI`            | `string username = null`, `int credit = 0` | Memperbarui nama akun dan tampilan saldo pada seluruh UI wallet.                                                                       |
| `PlaySFX`             | `string sfx`                               | Memutar efek suara berdasarkan nama audio yang diberikan.                                                                              |

| Nama Class             | Parameters / Fields            | Penjelasan                                                          |
| ---------------------- | ------------------------------ | ------------------------------------------------------------------- |
| `LoginRequest`         | `username`, `password`         | Data yang dikirim saat registrasi atau login.                       |
| `LoginResponse`        | `token`                        | Data token yang diterima setelah login berhasil.                    |
| `TokenResponse`        | `valid`, `username`, `credit`  | Data profil pengguna yang diterima dari server.                     |
| `ScaleResponse`        | `scale`, `weight`, `deposited` | Data timbangan yang diterima dari server.                           |
| `DepositRequest`       | `scaleId`                      | Data yang dikirim saat melakukan deposit timbangan.                 |
| `DepositResponse`      | `earned`, `totalCredit`        | Hasil deposit berupa kredit yang diperoleh dan total saldo terbaru. |
| `FirebaseLoginRequest` | `firebaseToken`                | Data token Firebase yang dikirim ke backend.                        |
| `PaymentResponse`      | `success`, `remaining`         | Hasil pembayaran permainan dan sisa saldo pengguna.                 |
| `ErrorMessage`         | `message`                      | Hasil pesan error dari backend.                                     |

2. `Settings.cs`
```csharp
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [Header("Sliders - Music")]
    public Slider musicSlider;
    public TextMeshProUGUI musicDisplayText;

    [Header("Sliders - SFX")]
    public Slider SFXSlider;
    public TextMeshProUGUI sfxDisplayText;

    private void OnEnable()
    {
        musicSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("Music", 1));
        SFXSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("SFX", 1));
    }

    public void SetVolume(bool isMusic)
    {
        if (isMusic)
        {
            PlayerPrefs.SetFloat("Music", musicSlider.value);
            if (musicDisplayText != null)
            musicDisplayText.text = ((int)(musicSlider.value * 100)).ToString();
        } else
        {
            PlayerPrefs.SetFloat("SFX", SFXSlider.value);
            if (sfxDisplayText != null)
            sfxDisplayText.text = ((int)(SFXSlider.value * 100)).ToString();
        }
    }
}

```

| Nama Function | Parameters     | Penjelasan                                                                                                                                                                                                                   |
| ------------- | -------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `OnEnable`    | ❌              | Dipanggil saat objek aktif. Mengambil nilai volume Music dan SFX dari `PlayerPrefs`, lalu mengatur posisi slider sesuai nilai yang tersimpan tanpa memicu event slider.                                                     |
| `SetVolume`   | `bool isMusic` | Mengubah dan menyimpan nilai volume ke `PlayerPrefs`. Jika `isMusic` bernilai `true`, volume musik diperbarui; jika `false`, volume efek suara (SFX) diperbarui. Selain itu, teks persentase volume pada UI juga diperbarui. |

3. `QRScanning.cs`
```csharp
using Proyecto26;
using QRCodeShareMain;
using System.Collections;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class QRScanning : MonoBehaviour
{
    public static QRScanning Instance {  get; private set; }
    public GameObject askForCameraButton;
    public WebCamTexture camTexture;
    public RawImage targetImage;
    private float scanCooldown = 0f;
    private Texture2D scanTexture;
    Coroutine checkPermissionRoutine = null;
    bool canScan;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        if (camTexture == null)
        {
            Initialize();
        } else
        {
            camTexture.Play();
            StartCoroutine(WaitForFreshFrames());
            StartCoroutine(FixCameraOrientation());
        }
    }

    private void OnDisable()
    {
        if (camTexture != null)
            camTexture.Stop();
        if (checkPermissionRoutine != null) StopCoroutine(checkPermissionRoutine);
		StopAllCoroutine();
    }

    public void Initialize()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
            if (checkPermissionRoutine != null) StopCoroutine(checkPermissionRoutine);
            checkPermissionRoutine = StartCoroutine(StartCamera());
            return;
        }

        if (camTexture != null)
        {
            camTexture.Play();
            StartCoroutine(WaitForFreshFrames());
            StartCoroutine(FixCameraOrientation());
            return;
        }

        WebCamDevice[] devices =
            WebCamTexture.devices;

        if (devices.Length == 0)
        {
            Debug.LogError("No camera found.");
            return;
        }

#if UNITY_ANDROID || UNITY_IOS

        bool foundRearCamera = false;

        for (int i = 0; i < devices.Length; i++)
        {
            if (!devices[i].isFrontFacing)
            {
                camTexture =
                    new WebCamTexture(
                        devices[i].name);

                foundRearCamera = true;
                break;
            }
        }

        if (!foundRearCamera)
        {
            camTexture =
                new WebCamTexture(
                    devices[0].name);
        }

#else

camTexture =
    new WebCamTexture(
        devices[0].name);

#endif

        targetImage.texture = camTexture;
        targetImage.material.mainTexture = camTexture;

        camTexture.Play();
        StartCoroutine(WaitForFreshFrames());
        StartCoroutine(FixCameraOrientation());
    }

    IEnumerator WaitForFreshFrames()
    {
        canScan = false;

        yield return new WaitForSeconds(1f);

        canScan = true;
    }

    IEnumerator StartCamera()
    {
        while (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            yield return null;
        }

        WebCamDevice[] devices =
            WebCamTexture.devices;

        if (devices.Length == 0)
        {
            Debug.LogError("No camera found.");
            yield break;
        }

    #if UNITY_ANDROID || UNITY_IOS

        bool foundRearCamera = false;

        for (int i = 0; i < devices.Length; i++)
        {
            if (!devices[i].isFrontFacing)
            {
                camTexture =
                    new WebCamTexture(
                        devices[i].name);

                foundRearCamera = true;
                break;
            }
        }

        if (!foundRearCamera)
        {
            camTexture =
                new WebCamTexture(
                    devices[0].name);
        }

    #else

    camTexture =
        new WebCamTexture(
            devices[0].name);

    #endif

        targetImage.texture = camTexture;
        targetImage.material.mainTexture = camTexture;

        camTexture.Play();
        StartCoroutine(WaitForFreshFrames());
        StartCoroutine(FixCameraOrientation());
        checkPermissionRoutine = null;
    }

    public void AskForCamera()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
        }
    }

    IEnumerator FixCameraOrientation()
    {
        yield return new WaitForSeconds(0.2f);

        if (camTexture == null)
            yield break;
        targetImage.rectTransform.localEulerAngles =
            new Vector3(
                0,
                0,
                -camTexture.videoRotationAngle
            );

        if (camTexture.videoVerticallyMirrored)
        {
            targetImage.rectTransform.localScale =
                new Vector3(1, -1, 1);
        }
    }

    void Update()
    {
        if (!canScan)
            return;
        if (MainMenu.Instance.loadingBlocker.activeSelf || MainMenu.Instance.modernScalePanel.gameObject.activeSelf)
        {
            if (camTexture != null && camTexture.isPlaying)
                camTexture.Pause();
            return;
        } else
        {
            if (camTexture != null && !camTexture.isPlaying)
            {
                camTexture.Play();
                StartCoroutine(WaitForFreshFrames());
                StartCoroutine(FixCameraOrientation());
            }
        }
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            askForCameraButton.SetActive(true);
            return;
        }
        askForCameraButton.SetActive(false);

        if (camTexture == null || !camTexture.isPlaying)
            return;

        scanCooldown -= Time.deltaTime;

        if (scanCooldown > 0)
            return;

        // scan every 0.5 seconds (prevents overload)
        scanCooldown = 0.5f;

        scanTexture = new Texture2D(
            camTexture.width,
            camTexture.height,
            TextureFormat.RGBA32,
            false
        );

        scanTexture.SetPixels32(
            camTexture.GetPixels32()
        );

        scanTexture.Apply();

        string result = QRCodeShare.ReadQRCodeImage(scanTexture);

        if (!string.IsNullOrEmpty(result))
        {
            Debug.Log("QR FOUND: " + result);

            OnQRDetected(result);
        }
    }

    void OnQRDetected(string scaleId)
    {
        camTexture.Stop();
        MainMenu.Instance.CheckScale(scaleId);
    }
}
```

| Nama Function          | Parameters       | Penjelasan                                                                                                                                        |
| ---------------------- | ---------------- | ------------------------------------------------------------------------------------------------------------------------------------------------- |
| `Awake`                | ❌                | Menginisialisasi singleton `QRScanning` dengan mengisi `Instance`.                                                                                |
| `OnEnable`             | ❌                | Dipanggil saat objek aktif. Memulai kamera jika belum ada, atau mengaktifkan kembali kamera yang sudah tersedia.                                  |
| `OnDisable`            | ❌                | Menghentikan kamera dan coroutine pengecekan izin saat objek dinonaktifkan.                                                                       |
| `Initialize`           | ❌                | Menginisialisasi kamera, meminta izin kamera jika diperlukan, memilih kamera belakang (mobile), lalu menampilkan hasil kamera ke UI.              |
| `WaitForFreshFrames`   | ❌                | Memberikan jeda 1 detik sebelum proses pemindaian QR dimulai agar frame kamera sudah stabil.                                                      |
| `StartCamera`          | ❌                | Menunggu hingga izin kamera diberikan, lalu menginisialisasi dan menjalankan kamera.                                                              |
| `AskForCamera`         | ❌                | Meminta izin akses kamera kepada pengguna jika belum diberikan.                                                                                   |
| `FixCameraOrientation` | ❌                | Menyesuaikan rotasi dan orientasi tampilan kamera agar sesuai dengan orientasi perangkat.                                                         |
| `Update`               | ❌                | Dipanggil setiap frame. Mengelola status kamera, memeriksa izin kamera, mengambil gambar dari kamera, dan melakukan pemindaian QR secara berkala. |
| `OnQRDetected`         | `string scaleId` | Dipanggil ketika QR Code berhasil dipindai. Menghentikan kamera dan mengirim ID timbangan ke `MainMenu` untuk diproses.                           |

4. `ScalePanel.cs`
```csharp
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
```

| Nama Function    | Parameters                        | Penjelasan                                                                                                                                                               |
| ---------------- | --------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| `Initialize`     | `string scale`, `int lastBalance` | Menginisialisasi panel timbangan dengan mengambil data berat dari server berdasarkan ID timbangan, lalu menghitung dan menampilkan kredit serta saldo pengguna.          |
| `FakeInitialize` | ❌                                 | Menginisialisasi panel menggunakan data acak (dummy) jika gagal mengambil data timbangan dari server. Biasanya digunakan untuk tahap tutorial atau fallback saat terjadi error. |
| `OnDisable`      | ❌                                 | Mereset timer dan status refresh ketika panel dinonaktifkan.                                                                                                             |
| `Update`         | ❌                                 | Memperbarui data timbangan secara berkala dari server setiap 2 detik atau menghasilkan data acak setiap 0,2 detik jika menggunakan mode dummy (`randomizedValue`).       |


5. `HighlightManager.cs`
```csharp
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighlightManager : MonoBehaviour
{
	public enum HighlightShape
	{
		CIRCLE = 0,
		SQUARE = 1
	}

	public RectTransform highlight;
	public Image highlightImage;
	public List<Button> everySingleButton;
	private List<bool> buttonLastState;

	[Header("Shapes")]
	public Sprite circleShape;
	public Sprite squareShape;

	private RectTransform targetPos;
    private Vector2 currentOffset;
    private bool followWidth;
    private bool followHeight;
    private bool isHighlighting;
    private const float SIZE_PADDING = 0f;
    public static HighlightManager Instance { get; private set; }

	private void Awake()
	{
        Instance = this;

        buttonLastState = new List<bool>();

        foreach (Button button in everySingleButton)
        {
            buttonLastState.Add(button.interactable);
        }

        highlight.gameObject.SetActive(false);
    }

    public void StartHighlighting(
        RectTransform target,
        Vector2 offset,
        HighlightShape shape = HighlightShape.SQUARE,
        bool followTargetWidth = false,
        bool followTargetHeight = false)
    {
        targetPos = target;
        currentOffset = offset;

        followWidth = followTargetWidth;
        followHeight = followTargetHeight;

        isHighlighting = true;

        highlight.gameObject.SetActive(true);

        highlightImage.sprite = shape == HighlightShape.CIRCLE
            ? circleShape
            : squareShape;

        CopyRectTransformSettings(target, highlight);
        // Position immediately
        highlight.position = target.position + (Vector3)offset;

        if (followWidth)
        {
            highlight.SetSizeWithCurrentAnchors(
                RectTransform.Axis.Horizontal,
                target.rect.width + SIZE_PADDING);
        }

        if (followHeight)
        {
            highlight.SetSizeWithCurrentAnchors(
                RectTransform.Axis.Vertical,
                target.rect.height + SIZE_PADDING);
        }

        // Disable buttons
        foreach (Button button in everySingleButton)
        {
            button.interactable = false;
        }
        Button targetButton = target.GetComponent<Button>();
        if (targetButton != null)
        {
            targetButton.interactable = true;
        }
    }

    private void Update()
    {
        if (!isHighlighting || targetPos == null)
            return;

        highlight.position = targetPos.position + (Vector3)currentOffset;

        if (followWidth)
        {
            highlight.SetSizeWithCurrentAnchors(
                RectTransform.Axis.Horizontal,
                targetPos.rect.width + SIZE_PADDING);
        }

        if (followHeight)
        {
            highlight.SetSizeWithCurrentAnchors(
                RectTransform.Axis.Vertical,
                targetPos.rect.height + SIZE_PADDING);
        }
    }

    public void EndHighlight()
    {
        isHighlighting = false;
        targetPos = null;

        highlight.gameObject.SetActive(false);

        for (int i = 0; i < everySingleButton.Count; i++)
        {
            everySingleButton[i].interactable = buttonLastState[i];
        }
    }

    public static void CopyRectTransformSettings(
    RectTransform source,
    RectTransform target)
    {
        target.anchorMin = source.anchorMin;
        target.anchorMax = source.anchorMax;
        target.pivot = source.pivot;
        target.anchoredPosition = source.anchoredPosition;
        target.sizeDelta = source.sizeDelta;
    }
}
```

| Nama Function               | Parameters                                                                                                                                                    | Penjelasan                                                                                                                                                                   |
| --------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `Awake`                     | ❌                                                                                                                                                             | Menginisialisasi singleton `HighlightManager`, menyimpan status awal seluruh tombol, dan menyembunyikan highlight saat awal permainan.                                       |
| `StartHighlighting`         | `RectTransform target`, `Vector2 offset`, `HighlightShape shape = HighlightShape.SQUARE`, `bool followTargetWidth = false`, `bool followTargetHeight = false` | Memulai efek highlight pada elemen UI tertentu, mengatur bentuk highlight, mengikuti ukuran target jika diperlukan, serta menonaktifkan interaksi tombol lain selain target. |
| `Update`                    | ❌                                                                                                                                                             | Memperbarui posisi dan ukuran highlight secara real-time agar selalu mengikuti target yang sedang disorot.                                                                   |
| `EndHighlight`              | ❌                                                                                                                                                             | Menghentikan highlight, menyembunyikan objek highlight, dan mengembalikan status interaksi semua tombol seperti semula.                                                      |
| `CopyRectTransformSettings` | `RectTransform source`, `RectTransform target`                                                                                                                | Menyalin pengaturan `RectTransform` dari objek sumber ke objek target, termasuk anchor, pivot, posisi, dan ukuran.                                                           |


6. `TutorialManager.cs`
```csharp
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { get; private set; }
    public TutorialState[] tutorialStates;
    public TutorialState endOfTutorial;
    int tutorialIndex;

    public Animator tutorialAnim;
    public TextMeshProUGUI tutorialText;

    public Button skipTutorial;
    public TextMeshProUGUI skipText;

    Button lastButtonForTutorial;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        tutorialIndex = -1;
    }

    public void CheckForTutorialStart(Button firstHighlightButton)
    {
        if (PlayerPrefs.GetInt("FirstTimePlaying", 0) == 0 && MainMenu.Instance.storedBalance < 5)
        {
            tutorialIndex = 0;
            tutorialStates[tutorialIndex].targetHighlight = firstHighlightButton.GetComponent<RectTransform>();
            StartTutorial();
        }
    }

    public void StartTutorial()
    {
        if (lastButtonForTutorial != null)
        {
            lastButtonForTutorial.onClick.RemoveListener(StartTutorial);
        }
        if (tutorialIndex >= tutorialStates.Length)
        {
            tutorialAnim.Play("TutorialDisappear", 0, 0f);
            tutorialText.text = endOfTutorial.tutorialText;
            HighlightManager.Instance.EndHighlight();
            tutorialIndex = -1;
            PlayerPrefs.SetInt("FirstTimePlaying", 1);
            return;
        }
        tutorialAnim.gameObject.SetActive(true);
        tutorialAnim.Play("TutorialAppear", 0, 0f);

        TutorialState state = tutorialStates[tutorialIndex];
        tutorialText.text = state.tutorialText;
        HighlightManager.Instance.StartHighlighting(state.targetHighlight, Vector2.zero, state.shape, state.followShapeWidth, state.followShapeHeight);

        if (state.enableSkipTutorial)
        {
            skipText.text = state.skipTutorialText;
            skipTutorial.gameObject.SetActive(true);
        } else
        {
            skipTutorial.gameObject.SetActive(false);
        }

        lastButtonForTutorial = state.targetHighlight.GetComponent<Button>();
        if (lastButtonForTutorial != null && lastButtonForTutorial != skipTutorial)
        {
            lastButtonForTutorial.onClick.AddListener(StartTutorial);
        }
        if (lastButtonForTutorial == skipTutorial)
        {
            lastButtonForTutorial = null;
        }

        tutorialIndex++;
    }

    public void SkipTutorial()
    {
        TutorialState current = GetActiveState();
        foreach (GameObject obj in current.enabledOnSkip)
        {
            Animator isThereAnim = obj.GetComponent<Animator>();
            if (isThereAnim != null)
            {
                isThereAnim.Play("PanelAppear", 0, 0f);
            }
            if (obj == MainMenu.Instance.modernScalePanel.gameObject)
            {
                MainMenu.Instance.modernScalePanel.FakeInitialize();
            }
            obj.SetActive(true);
        }
        foreach (GameObject obj in current.disabledOnSkip)
        {
            Animator isThereAnim = obj.GetComponent<Animator>();
            if (isThereAnim != null)
            {
                isThereAnim.Play("PanelDisappear", 0 ,0f);
            } else
            {
                obj.SetActive(false);
            }
        }
        StartTutorial();
    }

    public TutorialState GetActiveState()
    {
        return tutorialStates[tutorialIndex - 1];
    }

    public bool IsTutorialActive()
    {
        return tutorialIndex >= 0;
    }
}

[System.Serializable]
public class TutorialState
{
    public RectTransform targetHighlight;

    [TextArea(3, 10)]
    public string tutorialText;

    public HighlightManager.HighlightShape shape;

    public bool followShapeWidth;

    public bool followShapeHeight;

    public bool progressAfterScan;

    [Header("Skip Tutorial")]
    public bool enableSkipTutorial;
    [TextArea(3, 10)]
    public string skipTutorialText;
    public GameObject[] enabledOnSkip;
    public GameObject[] disabledOnSkip;
}
```

| Nama Function           | Parameters                    | Penjelasan                                                                                                                                                                           |
| ----------------------- | ----------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| `Awake`                 | ❌                             | Menginisialisasi singleton `TutorialManager` dengan mengisi `Instance`.                                                                                                              |
| `Start`                 | ❌                             | Mengatur nilai awal `tutorialIndex` menjadi `-1`, menandakan tutorial belum aktif.                                                                                                   |
| `CheckForTutorialStart` | `Button firstHighlightButton` | Memeriksa apakah pemain baru pertama kali bermain dan saldo kurang dari 5. Jika ya, memulai tutorial dari langkah pertama.                                                           |
| `StartTutorial`         | ❌                             | Menjalankan langkah tutorial saat ini, menampilkan teks tutorial, mengaktifkan highlight pada target UI, serta mengatur tombol yang akan melanjutkan tutorial ke langkah berikutnya. |
| `SkipTutorial`          | ❌                             | Menjalankan aksi yang telah ditentukan ketika pemain memilih melewati langkah tutorial tertentu, lalu melanjutkan ke langkah berikutnya.                                             |
| `GetActiveState`        | ❌                             | Mengembalikan data `TutorialState` yang sedang aktif saat ini.                                                                                                                       |
| `IsTutorialActive`      | ❌                             | Memeriksa apakah tutorial sedang berjalan atau tidak.                                                                                                                                |

| Nama Class      | Parameters / Fields                                                                                                                                                                   | Penjelasan                                                                                                                                                                                                 |
| --------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `TutorialState` | `targetHighlight`, `tutorialText`, `shape`, `followShapeWidth`, `followShapeHeight`, `progressAfterScan`, `enableSkipTutorial`, `skipTutorialText`, `enabledOnSkip`, `disabledOnSkip` | Menyimpan konfigurasi untuk satu langkah tutorial, termasuk target yang disorot, teks instruksi, bentuk highlight, pengaturan skip, serta objek yang diaktifkan atau dinonaktifkan saat tutorial dilewati. |

7. `CutoutMaskUI.cs`
```csharp
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class CutoutMaskUI : Image
{
    public override Material materialForRendering {
        get
        {
            Material material = new Material(base.materialForRendering);
            material.SetInt("_StencilComp", (int)CompareFunction.NotEqual);
            return material;
        }
    }
}
```

| Nama Function          | Parameters | Penjelasan                                                                                                                                             |
| ---------------------- | ---------- | ------------------------------------------------------------------------------------------------------------------------------------------------------ |
| `materialForRendering` | ❌          | Mengembalikan material khusus untuk proses rendering UI dengan mengubah pengaturan stencil agar area tertentu menjadi transparan (cutout/mask effect). |

8. `FirebaseManager.cs`

`FirebaseManager.cs` memperlukan **WebClientID** untuk berfungsi. Kamu bisa temukan ini di **google-services.json** atau melalui cara **Google Cloud** di - ["Langkah 1.6"](#langkah-16--setup-firebase).
```csharp
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Google;
using UnityEngine;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance { get; private set; }

    public FirebaseAuth Auth { get; private set; }

    public bool IsReady { get; private set; }

    private async void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        await InitializeFirebase();
    }

    private async Task InitializeFirebase()
    {
        var dependencyStatus =
            await FirebaseApp.CheckAndFixDependenciesAsync();

        if (dependencyStatus != DependencyStatus.Available)
        {
            Debug.LogError(
                "Firebase dependencies unavailable: " +
                dependencyStatus);

            return;
        }

        Auth = FirebaseAuth.DefaultInstance;

        try
        {
            GoogleSignIn.Configuration =
                new GoogleSignInConfiguration
                {
                    WebClientId =
                        "[webClientID]",

                    RequestIdToken = true
                };
        }
        catch (System.Exception e)
        {
            // Happens if GoogleSignIn.DefaultInstance
            // was already created before configuration.
            Debug.LogWarning(
                "Google Sign-In already configured: " +
                e.Message);
        }

        IsReady = true;

        Debug.Log("Firebase Manager Ready");
    }
}
```

| Nama Function        | Parameters | Penjelasan                                                                                                                                            |
| -------------------- | ---------- | ----------------------------------------------------------------------------------------------------------------------------------------------------- |
| `Awake`              | ❌          | Menginisialisasi singleton `FirebaseManager`, mencegah duplikasi objek, mempertahankan objek antar scene, dan memulai proses inisialisasi Firebase.   |
| `InitializeFirebase` | ❌          | Memeriksa dependensi Firebase, menginisialisasi Firebase Authentication, mengonfigurasi Google Sign-In, dan menandai Firebase sebagai siap digunakan. |

9. `AnimationEvents.cs`
```csharp
using UnityEngine;

[System.Serializable]
public class AnimationEvent
{
    public enum TypeOfEvent { Disable, Enable} // Modify this enum as much as you need.
    public TypeOfEvent eventType;
    public GameObject[] target;
}
public class AnimationEvents : MonoBehaviour
{
    public AnimationEvent[] events;
    void ActivateEvent(AnimationEvent selected) // Add the effects in your types here...
    {
        switch (selected.eventType)
        {
            case AnimationEvent.TypeOfEvent.Disable:
                foreach (GameObject target in selected.target)
                {
                    target.SetActive(false);
                }
                break;
            case AnimationEvent.TypeOfEvent.Enable:
                foreach (GameObject target in selected.target)
                {
                    target.SetActive(true);
                }
                break;
        }
    }

    public void StartEvent(int index)
    {
        ActivateEvent(events[index]);
    }

    public void StartAllEvent()
    {
        foreach (AnimationEvent anim in  events)
        {
            ActivateEvent(anim);
        }
    }
    
}
```

| Nama Function   | Parameters                | Penjelasan                                                                                                        |
| --------------- | ------------------------- | ----------------------------------------------------------------------------------------------------------------- |
| `ActivateEvent` | `AnimationEvent selected` | Menjalankan aksi berdasarkan jenis event yang dipilih, seperti mengaktifkan atau menonaktifkan GameObject target. |
| `StartEvent`    | `int index`               | Menjalankan satu event dari daftar `events` berdasarkan indeks yang diberikan.                                    |
| `StartAllEvent` | ❌                         | Menjalankan seluruh event yang terdapat dalam array `events`.                                                     |

| Nama Class        | Parameters / Fields   | Penjelasan                                                                                                     |
| ----------------- | --------------------- | -------------------------------------------------------------------------------------------------------------- |
| `AnimationEvent`  | `eventType`, `target` | Menyimpan konfigurasi sebuah event animasi, termasuk jenis aksi dan objek yang menjadi target.                 |
| `AnimationEvents` | `events`              | Mengelola dan menjalankan kumpulan `AnimationEvent`, biasanya dipanggil melalui Animation Event pada Animator. |

## 📱 TUTORIAL 7 - PEMBUATAN SCENE MAIN MENU

### Langkah 7.1 — Pembuatan Main Screen
**Main Screen** adalah tampilan awal disaat pemain pertama kali masuk. Pastikan pemain bisa main, mengatur volume, mendapat Credit, dan keluar dari permainan. Untuk tutorial ini kita memakai design sesuai Unity, bisa diganti sesuai keinginan.

1. Klik kanan di **Hierarchy → UI (Canvas) → Button - TextMeshPro**. Rename dan ganti teks di dalamnya menjadi `Main`.
2. Tambah script `MainMenu.cs`, `HighlightManager.cs`, dan `TutorialManager.cs` ke objek bebas. Di tutorial ini, kita akan masukkan ke `Canvas`.
3. Duplikat sebanyak tiga kali, lalu rename dan ganti teks menjadi `Credit`, `Pengaturan`, dan `Keluar`.
4. Posisikan dan desain **Button** sesuai dengan keinginan.
5. Tambahkan fungsi ke masing-masing tombol :
   
   `Main` :
   - `Canvas` > `MainMenu` > `**PlaySFX("button")**`
   - `Select Games Panel` > `GameObject` > `**SetActive(true)**` → Tambah fungsi ini setelah kamu membuat panelnya.
     
   `Credit` :
   - `Canvas` > `MainMenu` > `**PlaySFX("button")**`
   - `QR Scanning Panel` > `GameObject` > `**SetActive(true)**` → Tambah fungsi ini setelah kamu membuat panelnya.
  
   `Pengaturan` :
   - `Canvas` > `MainMenu` > `**PlaySFX("button")**`
   - `Settings Panel` > `GameObject` > `**SetActive(true)**` → Tambah fungsi ini setelah kamu membuat panelnya.
     
   `Keluar` :
   - `Canvas` > `MainMenu` > `**PlaySFX("button")**`
   - `Quit Panel` > `GameObject` > `**SetActive(true)**` → Tambah fungsi ini setelah kamu membuat panelnya.
  
6. Buat semacam tab yang berisi nama akun dan tombol untuk `Log Out`.

   `Log Out` :
   - `Canvas` > `MainMenu` > `**PlaySFX("button")**`
   - `Canvas` > `MainMenu` > `**LogOut()**`
    
7. Di `MainMenu.cs`, masukkan variable berikut :
	- Account Detail : `Account Detail Tab`
	- Account Name : `Account Name (TextMeshProUGUI)`

### Langkah 7.2 — Pembuatan Panel Template
Setiap panel yang akan kita buat akan memiliki beberapa hal yang sama, entah desain atau komponen.

1. Klik kanan di **Hierarchy → UI (Canvas) → Panel**. Rename menjadi `TEMPLATE`.
2. Di **Inspector**, tambah `Canvas Group`, `Animator`, dan `AnimationEvents.cs`.

3. **AnimationEvents.cs** :
   - Events : (EventType.Disable, `Template`) → Pastikan event menarget diri sendiri, dan EventType menjadi **Disable**.

4. Buat design yang diinginkan terlebih dahulu. Di tutorial ini, akan ada Tab yang isinya ada **TextMeshPro - Text (UI)** serta **Button** untuk menutup panel.
5. Untuk **Button**, jalankan :

   - `Board` > `GameManager` > `**PlaySFX("button")**`

   - `TEMPLATE` > `Animator` > `**Play("PanelDisappear")**`

6. Jika sudah selesai design, buka tab **Animation** dengan `Window → Animation → Animation`.
7. Animasi yang kita perlukan adalah `PanelAppear` dan `PanelDisappear`. `PanelAppear` adalah animasi panel yang mulai muncul, seperti **Fade In**. `PanelDisappear` adalah panel yang perlahan menghilang, seperti **Fade Out**. Pastikan di akhir frame `PanelDisappear`, ada **Animation Event** yang menjalankan `AnimationEvents.StartAllEvent()`. Serta, pastikan animasi juga mengkontrol **Canvas Group**, yaitu dibagian **Interactable**. Pastikan `PanelAppear` menyalakan **Interactable** di akhir frame dan `PanelDisappear` mematikan **Interactable** di awal frame.
8. Matikan **Loop Time** dari kedua **Animation Clip** tersebut dari file yang barusan dibuat.
9. Di **Animator**, yang diakses melalui `Window → Animation → Animator`, pastikan `PanelAppear` menjadi default clip yang mulai pertama kali. Klik kanan clip `PanelAppear`, lalu klik `Set as Layer Default State`.

### Langkah 7.3 — Pembuatan Panel Lain
Setelah membuat panel `TEMPLATE`, duplikat dan gunakan design tersebut untuk panel-panel lain. Berikut adalah detail yang diperlukan untuk masing-masing panel.

1. **Account Panel - Ask** → Tab ini hanya memiliki satu tombol, yaitu `Sign in with Google`. Pastikan tombol ini tersambung dengan `Canvas` > `MainMenu` > `**PlaySFX("button")**` dan `Canvas` > `MainMenu` > `**SignInGoogle()**`.

Masukkan panel ini ke script `MainMenu.cs` di variable `Sign In Panel`.

2. **Account Panel - Register (opsional)** → Tab ini berisi dua **TextMeshPro - InputField** yang dinamakan `Username Field` dan `Password Field`. Pastikan `Password Field` memiliki **Content Type** tipe **Password**. Serta, pastikan `Password Field` memiliki tombol kecil untuk menyembunyikan/menunjukkan password. Setelah itu, buat dua tombol untuk memulai tahap **Register** dan mengganti ke tahap **Login**.

   `Mulai Register` :
   - `Canvas` > `MainMenu` > `**PlaySFX("button")**`
   - `Canvas` > `MainMenu` > `**Register()**`
     
   `Ganti ke Login` :
   - `Canvas` > `MainMenu` > `**PlaySFX("button")**`
   - `Account Panel - Login` > `GameObject` > `**SetActive(true)**`
   - `Account Panel - Login` > `Animator` > `**Play("PanelAppear")**`
   - `Account Panel - Register` > `Animator` > `**Play("PanelDisappear")**`
  
Di `MainMenu.cs`, masukkan variable berikut :
- Register Tab : `Account Panel - Register (Animator)`
- Register Username Field : `Username Field (TMP_Input Field)`
- Register Password Field : `Password Field (TMP_Input Field)`
- Register Password Show : `Show Password (Image)`

3. **Account Panel - Login (opsional)** → Sama seperti **Account Panel - Register**, namun tombol yang mengganti ke tahap **Login** berubah menjadi **Register**, dan tombol untuk mulai tahap **Register** berubah menjadi **Login**.

   `Mulai Login` :
   - `Canvas` > `MainMenu` > `**PlaySFX("button")**`
   - `Canvas` > `MainMenu` > `**Login()**`
     
   `Ganti ke Register` :
   - `Canvas` > `MainMenu` > `**PlaySFX("button")**`
   - `Account Panel - Register` > `GameObject` > `**SetActive(true)**`
   - `Account Panel - Register` > `Animator` > `**Play("PanelAppear")**`
   - `Account Panel - Login` > `Animator` > `**Play("PanelDisappear")**`

Di `MainMenu.cs`, masukkan variable berikut :
- Login Tab : `Account Panel - Login (Animator)`
- Login Username Field : `Username Field (TMP_Input Field)`
- Login Password Field : `Password Field (TMP_Input Field)`
- Login Password Show : `Show Password (Image)`
  
4. **Quit Panel** → Memiliki setidaknya 2 tombol untuk konfirmasi ingin keluar atau tidak.

   `Yes - Quit` :
   - `Canvas` > `MainMenu` > `**PlaySFX("button")**`
   - `Canvas` > `MainMenu` > `**QuitGame()**`
     
   `No - Quit` :
   - `Canvas` > `MainMenu` > `**PlaySFX("button")**`
   - `QuitPanel` > `Animator` > `**Play("PanelDisappear)**`

5. **Settings Panel** → Memiliki 2 slider yang mengkontrol volume musik dan sound effect. Tambahkan script `Settings.cs` di `Settings Panel`. Tarik kedua slider ke variable yang diperlukan di `Settings.cs`. Pastikan juga ada tombol untuk menutup panel.

   `Music Slider` **OnValueChanged()** :
   - ``Settings Panel`` > `Settings` > `**SetVolume(true)**`
  
   `SFX Slider` **OnValueChanged()** :
   - ``Settings Panel`` > `Settings` > `**SetVolume(false)**`
  
6. **Choose Credit Panel** → Memiliki 2 tombol untuk memilih apakah ingin langsung menyambungkan ke timbangan atau menggunakan kode QR. Disini ada **Notification Tab**, masukkan ke panel ini. Jangan lupa untuk memasukkan **Animator** dan **TextMeshProUGUI - Text (UI)** ke dalam `MainMenu.cs` di variable `notifcationQRText` dan `notificationQRAppear`. Tambahkan animasi dengan nama `TurnAppear` dan **Loop Time** dimatikan.

   `Automatic Button` **OnValueChanged()** :
   - `Canvas` > `MainMenu` > `**PlaySFX("button")**`
   - `Canvas` > `MainMenu` > `**AutomaticScale()**`
  
   `Manual Button` **OnValueChanged()** :
   - `Canvas` > `MainMenu` > `**PlaySFX("button")**`
   - `QR Scanning Panel` > `GameObject` > `**SetActive(true)**`
   - `Choose Credit Panel` > `Animator` > `**Play("PanelDisappear")**`

8. **QR Scanning Panel** → Memiliki **Raw Image** untuk menunjukkan kamera, tombol untuk menanyakan apabila aplikasi tidak punya akses ke kamera, dan tombol menutup panel. Tambahkan `QRScanning.cs` ke objek `QR Scanning Panel`. Masukkan variable **Raw Image** ke `targetImage` dan tombol menanyakan ke `askForCameraButton`. Disini juga ada **Notification Tab**, duplikat dari yang lain, lalu masukkan ke panel ini. Jangan lupa untuk memasukkan **Animator** dan **TextMeshProUGUI - Text (UI)** ke dalam `MainMenu.cs` di variable `notifcationQRText` dan `notificationQRAppear`.

Di `MainMenu.cs`, masukkan variable berikut :
- Camera QR Menu : `QR Scanning Panel (Animator)`
- Notification QR Appear : `Notification QR (Animator)`

9. **Conversion Credit Panel** → Memiliki 4 **TextMeshPro - Text (UI)** yang menunjukkan jumlah berat, konversi berat ke Credit, jumlah Credit sebelum konversi dan sesudah konversi. Tambahkan `ScalePanel.cs` ke objek `Conversion Credit Panel`. Masukkan variable :
   - Scale Amount : `Scale Amount Text` → Teks yang menunjukkan jumlah berat.
   - Credit Amount : `Credit Amount Text` → Teks yang menunjukkan jumlah konversi ke Credit.
   - Old Balance : `Old Balance Text` → Teks yang menunjukkan jumlah Credit sebelum konversi.
   - New Balance : `New Balance Text` → Teks yang menunjukkan jumlah Credit sesudah konversi.
   - Self Anim : `Conversion Credit Panel` → Animator objek itu sendiri.
   - Value Update : `OPSIONAL` → Bebas mau diisi animasi apa, ini akan jalan setiap isi dari timbangan ter-update.
   
   Disini juga ada tombol `Claim` dan tombol `Back`.  Masukkan fungsi berikut :
   
   `Claim` :
   - `Canvas` > `MainMenu` > `**PlaySFX("button")**`
   - `Canvas` > `MainMenu` > `**DepositScale()**`
     
   `Back` :
   - `Canvas` > `MainMenu` > `**PlaySFX("button")**`
   - `Canvas` > `MainMenu` > `**StopScaleConnection()**`

Untuk yang terakhir, duplikat **Notification Tab**, lalu masukkan ke panel ini. Jangan lupa untuk memasukkan **Animator** dan **TextMeshProUGUI - Text (UI)** ke dalam `MainMenu.cs` di variable `notifcationQRText` dan `notificationQRAppear`. 
  
Di `MainMenu.cs`, masukkan variable berikut :
- Modern Scale Panel : `Conversion Credit Panel (Scale Panel)`

10. **Select Games Panel** → Memiliki 4 tombol yang digunakan untuk memilih mode permainan yang ingin dimainkan. Setiap tombol akan membuka **Select Games Sub-Panel** untuk jenis game yang ditunjukkan di tombolnya. Serta ada tombol `Back`.

11. **Select Games Sub-Panel** → Memiliki 2 tombol yang menunjukkan `Main` dan `Kembali`, serta teks yang menunjukkan jumlah Credit yang dimiliki pemain dan teks yang menunjukkan nama permainan/deskripsi dari permainan yang dimainkan. Pastikan ada 4 panel agar setiap tombol di `Select Games Panel` bisa menyalakan panel miliknya.

   `Main` :
   - `Canvas` > `MainMenu` > `**PlaySFX("button")**`
   - `Canvas` > `MainMenu` > `**PlayGame(1)**` → Untuk "Congklak", indexnya adalah 1. Untuk mode lain, button-nya harus dimatikan **Interactable**.

Setelah membuat keempat panel ini, kembali ke **Select Games Panel**, lalu ke 4 tombol tersebut. Tambahkan fungsi berikut :

   `Congklak Mode` :
   - `Canvas` > `TutorialManager` > `**CheckForTutorialStart(Back Button)**` → Pilih tombol **Back** yang ada di Congklak Sub-Panel.

   `Gasing Mode` :
   - `Canvas` > `TutorialManager` > `**CheckForTutorialStart(Back Button)**` → Pilih tombol **Back** yang ada di Gasing Sub-Panel.

   `Ketapel Mode` :
   - `Canvas` > `TutorialManager` > `**CheckForTutorialStart(Back Button)**` → Pilih tombol **Back** yang ada di Ketapel Sub-Panel.

   `Kelereng Mode` :
   - `Canvas` > `TutorialManager` > `**CheckForTutorialStart(Back Button)**` → Pilih tombol **Back** yang ada di Kelereng Sub-Panel.

Masukkan teks yang menunjukkan jumlah Credit dalam script `MainMenu.cs` di variable `Every Currency Showcase`. Dalam tutorial, harus ada 4 **TextMeshProUGUI**.

12. **Highlight** → Digunakan untuk menunjukkan tombol yang ingin difokuskan disaat tutorial.
    a. Klik kanan di **Hierarchy → UI (Canvas) → Image**.
    b. Ganti sprite menjadi `9-Sliced`, apabila tidak ada, pilih `Background`.
    c. Tambah komponen `Mask` dan matikan **Show Mask Graphic**.
    d. Klik kanan di **Highlight → UI (Canvas) → Image**.
    e. Ganti warnanya menjadi warna hitam dengan sedikit transparan.
    f. Besarkan bentuknya hingga menutupi semua layar. Untuk lebih aman, buat `Width` dan `Height` menjadi 9999.
    g. Matikan seluruh objek **Highlight**, akan kita pakai disaat kita membuat sistem tutorial.
    h. Di `HighlightManager.cs` yang ada di **Canvas**, masukkan variable :
	- Highlight : `Highlight (Rect Transform)`
 	- Highlight Image : `Highlight (Image)`
	- Every Single Button : "Semua **Button** kecuali `Skip Tutorial` → **Sebaiknya lakukan ini setelah semua tombol sudah dibuat.**
 	- Circle Shape : `Circle Sprite.png` → Masukkan sprite berbentuk lingkaran.
  	- Square Shape : `Square Sprite.png` → Masukkan sprite berbentuk persegi.

14. **Tutorial Tab** → Muncul ketika tutorial sedang jalan. Hanya muncul di bagian atas layar. Beri animasi dengan nama `TutorialAppear` untuk tutorial yang muncul dan update teks, dan `TutorialDisappear` untuk tutorial yang selesai, dan akan menghilang perlahan. Pastikan `Tutorial Tab` memiliki **TextMeshPro - Text (UI)** di dalamnya. Tambahkan juga tombol agar pemain bisa melewati tutorial, dengan **TextMeshPro - Text (UI)**.

    `Skip Tutorial` :
   - `Canvas` > `MainMenu` > `**PlaySFX("button")**`
   - `Canvas` > `TutorialManager` > `**SkipTutorial()**`

Jangan lupa untuk mengisi variable di `TutorialManager.cs` yang ada di **Canvas**.
- Tutorial Anim : `Tutorial Tab (Animator)`
- Tutorial Text : `Tutorial Text (TextMeshProUGUI)`
- Skip Tutorial : `Skip Tutorial (Button)`
- Skip Text : `Skip Text (TextMeshProUGUI)`

14. **Loading Blocker** → Untuk yang terakhir, ini hanya sebuah panel yang menutupi semua input tombol. Buat panel melalui **Hierarchy → UI (Canvas) → Panel**, lalu tambah animasi **Fade In** dari animator yang sama dengan `TEMPLATE`. Masukkan panel ini dalam script `MainMenu.cs` di variable `Loading Blocker`.

### Langkah 7.4 — Isi Variable Kosong
1. `MainMenu.cs` :
   - Show Password : `Show Sprite` → Masukkan sprite yang menunjukkan bahwa password itu kelihatan, misal gambar mata.
   - Hide Password : `Hide Sprite` → Masukkan sprite yang menunjukkan bahwa password itu tersembunyi, misal gambar mata yang di silang.
   - Transition Settings : `Fade` → Pilih transisi dari template yang ada.
   - Base Link : `https://backend.com' → Masukkan link backend yang kamu punya. Pastikan ada [https://] di bagian awal. Untuk backend, akan diajarkan pembuatannya di [Tutorial 8](#-tutorial-8---pembuatan-backend).
2. `TutorialManager.cs` :
   - Tutorial States :

| Index | Target Highlight | Shape | Progress After Scan | Skip Tutorial | Skip Text |
|-------|-----------------|--------|--------------------|--------------|-----------|
| 0 | Tombol tutup popup Credit | Square | ❌ | ❌ | - |
| 1 | Tombol tutup tab | Square | ❌ | ❌ | - |
| 2 | Tombol Scan QR | Circle | ❌ | ❌ | - |
| 3 | Panel Scan QR | Square | ✅ | ✅ | Saya belum punya kode QR. |
| 4 | Tombol Klaim Credit | Square | ❌ | ❌ | - |

Untuk index 3, pastikan :
- Disable on Skip : { `QR Scanning Panel` }
- Enable on Skip : { `Credit Conversion Panel` }

## 🌐 TUTORIAL 8 - PEMBUATAN BACKEND
Backend diperlukan untuk mengirim dan menerima data dari timbangan. Untuk itu, kita perlu membuatnya menggunakan `Visual Studio Code`.

1. Buat projek baru dengan **Node.js + Express**.
2. Buat file baru bernama `server.js`.
3. Install semua package ini melalui **Terminal** :

```csharp
npm install express mongoose bcrypt jsonwebtoken dotenv express-rate-limit firebase-admin
```

3. Tulis :

```csharp
const express = require("express");
const mongoose = require("mongoose");
const bcrypt = require("bcrypt");
const jwt = require("jsonwebtoken");
require("dotenv").config();
const rateLimit = require("express-rate-limit");
const admin = require("firebase-admin");
const serviceAccount = JSON.parse(process.env.FIREBASE_SERVICE_ACCOUNT);

admin.initializeApp({
    credential: admin.credential.cert(serviceAccount)
});

mongoose.connect(process.env.MONGO_URL);

const loginLimiter =
    rateLimit({
        windowMs: 15 * 60 * 1000,
        max: 20
    });

const User = mongoose.model("User", {
    username: {
        type: String,
        unique: true,
        sparse: true
    },

    passwordHash: String,

    firebaseUid: {
        type: String,
        unique: true,
        sparse: true
    },

    email: String,

    credit: {
        type: Number,
        default: 0
    }
});

const app = express();
app.use(express.json());

const scales = {};

//Scale counting
app.post("/scale/:id", (req, res) => {

    const id = req.params.id;
    const weight = Number(req.body.weight);

    if (weight === 0) {
        scales[id] = {
            weight: 0,
            deposited: false
        };
    }
    else {
        scales[id] = {
            weight,
            deposited:
                scales[id]?.deposited ?? false
        };
    }

    res.json({
        success: true
    });
});

app.get("/scale/:id", (req, res) => {

    const id = req.params.id;

    const scale = scales[id];

    if (!scale) {
        return res.status(404).json({
            message: "Scale not found"
        });
    }

    res.json({
        scale: id,
        weight: scale.weight,
        deposited: scale.deposited
    });
});

app.post("/deposit", auth, async (req, res) => {
    try {

        const scaleId = req.body.scaleId;

        const scale = scales[scaleId];

        if (!scale) {
            return res.status(404).json({
                message: "Scale not found"
            });
        }

        if (scale.deposited) {
            return res.status(400).json({
                message: "Already deposited"
            });
        }

        if (scale.weight < 100) {
            return res.status(400).json({
                message: "Not enough weight"
            });
        }

        const credit =
            Math.floor(scale.weight / 100);

        const user =
            await User.findById(
                req.user.userId
            );

        user.credit += credit;

        await user.save();

        scale.deposited = true;

        res.json({
            earned: credit,
            totalCredit: user.credit
        });
    } 
    catch (err) {
        console.error(err);

        res.status(500).json({
            message: "Server error"
        });
    }
});

//Account creation
app.post("/register", async (req, res) => {

    if (
        !req.body.username ||
        req.body.username.length < 3
    ) {
        return res.status(400).json({
            message:
                "Username too short"
        });
    }

    if (
        !req.body.password ||
        req.body.password.length < 6
    ) {
        return res.status(400).json({
            message:
                "Password too short"
        });
    }

    const existing =
        await User.findOne({
            username: req.body.username
        });

    if (existing) {
        return res
            .status(400)
            .json({
                message: "Username taken"
            });
    }

    const passwordHash =
        await bcrypt.hash(
            req.body.password,
            10
        );

    const user = await User.create({
        username: req.body.username,
        passwordHash
    });

    const token = jwt.sign(
        {
            userId: user._id,
            username: user.username
        },
        process.env.JWT_SECRET,
        {
            expiresIn: "7d"
        }
    );

    res.json({ token });
});

app.post("/login", loginLimiter, async (req, res) => {
    try {

        const user = await User.findOne({
            username: req.body.username
        });

        if (
            !user ||
            !(await bcrypt.compare(
                req.body.password,
                user.passwordHash
            ))
        ) {
            return res.status(401)
                .send("Invalid login");
        }

        const token = jwt.sign(
            {
                userId: user._id,
                username: user.username
            },
            process.env.JWT_SECRET,
            {
                expiresIn: "7d"
            }
        );

        res.json({ token });

    } catch (err) {

        console.error(err);

        res.status(500).json({
            message: "Server error"
        });

    }
});
function auth(req, res, next)
{

    const header =
        req.headers.authorization;

    if (!header)
        return res.sendStatus(401);

    const token =
        header.replace(
            "Bearer ",
            ""
        );

    try {

        req.user =
            jwt.verify(
                token,
                process.env.JWT_SECRET
            );

        next();

    } catch {

        return res.sendStatus(401);

    }
}

app.get("/profile", auth, async (req, res) => {

    try {

        const user =
            await User.findById(req.user.userId);

        if (!user) {
            return res.status(404).json({
                message: "User not found"
            });
        }

        // If we reach here, token is valid (auth already passed)
        res.json({
            valid: true,
            username: user.username,
            credit: user.credit
        });

    } catch (err) {

        console.error(err);

        res.status(500).json({
            message: "Server error"
        });

    }

});

//firebase account
app.post(
    "/firebase-login",
    async (req, res) => {
        try {
            const { firebaseToken } =
                req.body;

            const decoded =
                await admin
                    .auth()
                    .verifyIdToken(
                        firebaseToken
                    );


            const uid =
                decoded.uid;

            const email =
                decoded.email;

            const username =
                decoded.name ||
                email.split("@")[0];

            let user =
                await User.findOne({
                    firebaseUid: uid
                });

            if (!user) {
                user =
                    await User.create({
                        firebaseUid: uid,
                        email,
                        username,
                        credit: 0
                    });
            }

            const jwtToken =
                jwt.sign(
                    {
                        userId: user._id
                    },
                    process.env.JWT_SECRET
                );

            res.json({
                token: jwtToken
            });
        }
        catch (error) {
            console.error("Firebase login error:");
            console.error(error);

            res.status(401).json({
                message: error.message
            });
        }
    });

app.post("/payment", auth, async (req, res) => {
    try {
        const user = await User.findById(req.user.userId);

        if (!user) {
            return res.status(404).json({
                success: false,
                message: "User not found"
            });
        }

        // Not enough credit
        if (user.credit < 5) {
            return res.status(400).json({
                success: false,
                message: "Not enough credit",
                remaining: user.credit
            });
        }

        user.credit -= 5;
        await user.save();

        return res.json({
            success: true,
            remaining: user.credit
        });

    } catch (err) {
        console.error(err);

        return res.status(500).json({
            success: false,
            message: "Server error"
        });
    }
});

app.listen(3000, () => {
    console.log("Server running");
});
```

| Nama Endpoint | Parameters | Penjelasan |
| ------------- | ---------- | ---------- |
| `POST /scale/:id` | **URL Param:** `id`<br>**Body:** `weight` | Mencatat atau memperbarui berat objek pada timbangan tertentu secara *real-time*. Jika beratnya 0, status timbangan di-reset (belum didepositkan). |
| `GET /scale/:id` | **URL Param:** `id` | Mengambil data kondisi timbangan saat ini berdasarkan ID-nya, termasuk informasi berat terbaru dan apakah berat tersebut sudah diklaim/didepositkan. |
| `POST /deposit` | **Header:** `Authorization` (JWT)<br>**Body:** `scaleId` | Memungkinkan pengguna terautentikasi untuk mengonversi berat yang ada di timbangan menjadi saldo kredit akun mereka (setiap 100 unit berat = 1 kredit), lalu menandai timbangan tersebut sudah diklaim (`deposited: true`). |
| `POST /register` | **Body:** `username`, `password` | Mendaftarkan pengguna baru dengan melakukan validasi panjang input, memastikan *username* belum terpakai, melakukan *hashing* password, menyimpannya ke database MongoDB, dan langsung mengembalikan JWT token. |
| `POST /login` | **Body:** `username`, `password` | Memverifikasi kredensial pengguna lokal menggunakan `bcrypt`. Jika cocok, *endpoint* ini akan mengembalikan JWT token untuk akses sesi. Dilindungi oleh `loginLimiter` untuk mencegah serangan *brute-force*. |
| `auth` | **Header:** `Authorization` (JWT) | Fungsi perantara *(middleware)* untuk mengamankan *endpoint*. Ia memeriksa dan memverifikasi keaslian JWT token yang dikirim di header. Jika valid, data pengguna disimpan ke `req.user` dan akses dilanjutkan; jika tidak, ia langsung menolak akses dengan status 401. |
| `GET /profile` | **Header:** `Authorization` (JWT) | Mengambil informasi profil dari pengguna yang sedang login (seperti *username* dan total saldo kredit) setelah lolos verifikasi *middleware* `auth`. |
| `POST /firebase-login` | **Body:** `firebaseToken` | Mengautentikasi pengguna menggunakan pihak ketiga melalui Firebase. Server memverifikasi token Firebase tersebut, lalu otomatis mendaftarkan pengguna baru ke MongoDB jika akun belum ada, kemudian mengembalikan JWT token lokal. |
| `POST /payment` | **Header:** `Authorization` (JWT) | Memproses transaksi pembayaran internal dengan memotong saldo kredit milik pengguna sebanyak 5 unit, selama saldo pengguna mencukupi. |

4. Di foldernya, buat file dengan nama `.env`. Lalu, buka dengan **Notepad** atau aplikasi tulis lain.
5. Tulis :

```csharp
MONGO_URL=XXX
JWT_SECRET=XXX
FIREBASE_SERVICE_ACCOUNT=XXX
```

6. Isi variable yang kosong sesuai dengan projek-mu, pastikan untuk menjaga file ini, dan tidak upload kemanapun.
   - MONGO_URL : Bisa ditemukan di website [cloud.mongodb.com/](cloud.mongodb.com). Buat akun, cluster baru, lalu connect. Kamu akan mendapat semacam teks, lalu masukkan ke `.env` ini.
   - JWT_SECRET : Ini digunakan untuk mengacak password untuk pengguna aplikasi-mu. Buat sepanjang mungkin dan seacak mungkin. Pastikan tidak ada yang tahu teks ini.
   - FIREBASE_SERVICE_ACCOUNT : Ini adalah `google-service.json` yang dibuat di tutorial sebelumnya. Buka file tersebut, copy seluruh teksnya, lalu masukkan ke `.env`. Pastikan teks muncul dalam satu baris, apabila ada linebreak, akan muncul error di console.
  
7. Di Terminal, jalankan :

```csharp
node server.js
```

8. Backend-mu sekarang sudah jalan secara lokal!
9. Untuk mencoba sistem backend tanpa timbangan, gunakan [**Postman**](www.postman.com).

## 📷 TUTORIAL 9 - PEMBUATAN KODE QR
Sebelum memulai testing, pemain harus memiliki Credit. Dan, cara untuk mendapat Credit adalah untuk mengakses timbangan.
1. Pastikan Anda tahu apa ID timbangan yang ingin diakses. Contohnya seperti "TimbanganID1". Apabila masih menggunakan [**Postman**](www.postman.com), buat ID sendiri lalu sambungkan melalui **Postman**.
2. Pergi ke [QR Code Dynamic](https://qrcodedynamic.com/qr/text) lalu generate kode QR yang isinya hanya ID itu saja.
3. Dalam game, coba scan kode QR tersebut. Apabila gagal, timbangan itu belum disambungkan atau sudah di ambil Creditnya sebelumnya.
4. Di **Postman**, masukkan :

```csharp
POST https://backend.com/scale/{SCALEID}
```

Pastikan `SCALEID` diganti dengan ID yang ingin disambungkan.

5. Klik `Body`, lalu `raw`, tulis berikut :

```csharp
{
    "weight": 500
}
```

Apabila ada yang mengganti angka dari suatu ID, maka timbangan itu akan selamanya tersambung. Jadi, jika kita mengisi 500 gram di `TimbanganID1`, angka tersebut akan tetap 500 gram di permainan.

5. Setiap ada orang yang selesai ambil Credit dari timbangan, pastikan timbangan tersebut harus dihilangkan beratnya, seperti `"weight": 0`. Hal ini agar tidak ada pemain yang menggunakan berat yang sama, sehingga bisa berulang-ulang mendapat Credit.

## 🔧 TUTORIAL 10 - TESTING GAME
Setelah membuat backend dan kode QR, sudah saatnya untuk testing game.

-> Urutan memainkan game :
1. Pergi ke scene `MainMenu`, lalu pencet 🞂 Play.
2. Diawali dari klik `Play`, lalu pilih salah satu mode permainan.
3. Akan ada tutorial yang muncul, ikutin alurnya hingga scan kode QR.
4. Scan kode QR yang sudah kamu buat.
5. Apabila berhasil, coba ambil hasilnya di game.
6. Jika Credit menambah, coba memainkan "Congklak".
7. Tes aturan dan mekanik.
8. Jika sudah selesai, tes fitur-fitur lain, seperti Settings, Turbo Mode, dan Tombol-tombol lain.

## 🎉 AKHIR TUTORIAL
Selamat! Anda telah mencapai akhir dari tutorial ini. Tutorial ini menggunakan Unity 6.3 LTS (6000.3.8f1).
