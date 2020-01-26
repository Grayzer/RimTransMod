Public Class Form1

    Friend SteamLib As String = ""
    Friend SteamEnable As Boolean = False
    Friend PathSteam As String = ""
    Friend SteamRimEnable As Boolean = False
    Friend RimEnable As Boolean = False
    Friend PathSteamRim As String = ""
    Friend PathRim As String = ""
    Friend SteamRimWorkshopEnable As Boolean = False
    Friend PathSteamRimWorkshop As String = ""
    Friend PathLang As String = ""
    Friend PathMod As String = ""
    Friend RimSteamMod As String()
    Friend RimLocMod As String()
    Friend RimSteamModFullPath As String()
    Friend RimLocModFullPath As String()
    Friend RimSteamModName As String()
    Friend RimLocModName As String()

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim res As DialogResult = FolderBrowserDialog1.ShowDialog()
        If res = DialogResult.OK Then
            PathRim = FolderBrowserDialog1.SelectedPath
            If IO.File.Exists(PathRim + "\RimWorldWin64.exe") Or IO.File.Exists(PathRim + "\RimWorldWin.exe") Then
                RimEnable = True
                TextBox1.Text = PathRim
                ToolStripStatusLabel4.Text = "Готово к работе."
            Else
                RimEnable = False
                ToolStripStatusLabel4.Text = "Это не каталог игры RimWorld."
                TextBox1.Clear()
            End If
            RimLang()
        End If

    End Sub

    Public Function RimLang() As Boolean

        If SteamRimEnable Then PathLang = PathSteamRim + "\Mods\Core\Languages\"
        If RimEnable Then PathLang = PathRim + "\Mods\Core\Languages\"
        Dim lll As Integer = PathLang.Length
        Dim ddd = IO.Directory.GetDirectories(PathLang)
        Dim xxx = ddd.Count
        For i = 0 To xxx - 1
            ddd(i) = ddd.ElementAt(i).Substring(lll)
        Next
        If SteamRimEnable Or RimEnable Then
            ComboBox1.Items.Clear()
            ComboBox1.Items.AddRange(ddd)
            ComboBox1.SelectedIndex = 0
            Return (True)
        Else
            Return (False)
            ComboBox1.Items.Clear()
            ComboBox1.Items.Add("English")
            ComboBox1.SelectedIndex = 0
        End If

    End Function

    Public Function NameMod(FullPathMod As String) As String
        Dim ModName As String = ""
        Dim doc As XDocument = XDocument.Load(FullPathMod + "\About\About.xml")

        ModName = doc.Element("ModMetaData").Element("name").Value.ToString

        Return (ModName)
    End Function

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim aaa As String = ""
        Dim bbb As Integer = 0

        If My.Computer.Registry.GetValue("HKEY_CURRENT_USER\Software\Valve\Steam", "SteamPath", Nothing) Is Nothing Then
            SteamEnable = False
            ToolStripStatusLabel1.Text = "Steam: не найден."
        Else
            SteamEnable = True
            PathSteam = My.Computer.Registry.GetValue("HKEY_CURRENT_USER\Software\Valve\Steam", "SteamPath", Nothing)
            ToolStripStatusLabel1.Text = "Steam: установлен."
        End If

        If My.Computer.Registry.GetValue("HKEY_CURRENT_USER\Software\Valve\Steam\Apps\294100", "Installed", Nothing) Is Nothing Then
            SteamRimEnable = False
            ToolStripStatusLabel2.Text = "Rimword: не найден."
        Else
            If My.Computer.Registry.GetValue("HKEY_CURRENT_USER\Software\Valve\Steam\Apps\294100", "Installed", Nothing) = 1 Then
                SteamRimEnable = True
                ToolStripStatusLabel2.Text = "Rimword: установлен."

                Dim zzz = My.Computer.FileSystem.ReadAllText(PathSteam + "\config\config.vdf")

                bbb = InStr(zzz, "BaseInstallFolder_1")
                If bbb > 0 Then
                    aaa = Trim(zzz.Substring(bbb + 22))
                    bbb = InStr(aaa, """")
                    aaa = Mid(aaa, 1, bbb - 1)
                    While InStr(aaa, "\\")
                        bbb = InStr(aaa, "\\")
                        aaa = aaa.Remove(bbb, 1)
                    End While
                    PathSteamRim = aaa + "\SteamApps\common\RimWorld"
                    PathSteamRimWorkshop = aaa + "\SteamApps\workshop\content\294100"

                    If IO.File.Exists(PathSteamRim + "\Version.txt") Then
                        Dim vvv As String = My.Computer.FileSystem.ReadAllText(PathSteamRim + "\Version.txt")
                        vvv = vvv.Trim(vbCr, vbLf)
                        ToolStripStatusLabel3.Text = "Версия игры: " + vvv
                    Else
                        ToolStripStatusLabel3.Text = "Версия игры: неизвестна"
                    End If


                    'TextBox1.Text = PathSteamRim

                End If
            End If
        End If

        RimLang()

    End Sub



    Private Sub CheckedListBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CheckedListBox2.SelectedIndexChanged

        Dim lll As Integer = 0
        Dim CurModPath As String = ""

        If SteamRimEnable Then CurModPath = PathSteamRim + "\Mods"
        If RimEnable Then CurModPath = PathRim + "\Mods"
        lll = CurModPath.Length
        Dim aaa = IO.Directory.GetDirectories(CurModPath)
        Dim xxx = aaa.Count
        Dim zzz As String = ""

        If SteamRimEnable Then
            RimSteamModFullPath = IO.Directory.GetDirectories(CurModPath)
            RimSteamMod = aaa
        End If

        If RimEnable Then
            RimLocModFullPath = aaa
            RimLocMod = aaa
        End If

        For i = 0 To xxx - 1
            If SteamRimEnable Then
                RimSteamMod(i) = aaa(i).Substring(lll + 1)
            End If

            If RimEnable Then
                RimLocMod(i) = aaa(i).Substring(lll + 1)
            End If

        Next

        If SteamRimEnable And CheckedListBox2.GetItemCheckState(0) = CheckState.Checked Then
            ListBox1.Items.Clear()
            ListBox1.Items.AddRange(RimSteamMod)
        Else
            ListBox1.Items.Clear()
        End If

        ListBox1.Refresh()


    End Sub

    Private Sub ListBox1_SelectedValueChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedValueChanged
        Dim ModPath As String = ""
        Dim sel As Integer = 0

        sel = ListBox1.SelectedIndex
        ModPath = RimSteamModFullPath(sel)
        TextBox2.Text = NameMod(ModPath)
    End Sub
End Class
