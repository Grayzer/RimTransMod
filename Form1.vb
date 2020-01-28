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
    Friend ModNames(499) As (String, String)
    Friend DefsList As String()
    Friend SubDefsList As String()


    Public Function FillDefNames() As Boolean
        Dim fname As String
        Dim idx = ListBox1.SelectedIndex                                                            'специально делаю именно так, с прицелом на будущее
        Dim idxMod = ListBox2.SelectedItem
        Dim idxDefs = ListBox3.SelectedItem
        fname = ModNames(idx).Item2 + "\Defs\" + idxMod + "\" + idxDefs
        Dim doc As XDocument = XDocument.Load(fname, System.StringComparison.CurrentCultureIgnoreCase)

        Dim xdef = doc.Root.Elements                                                                'получаем полный список ветвей
        Dim idef = xdef.Count                                                                       'получаем кол-во ветвей
        ListBox4.Items.Clear()
        For xroot = 0 To idef - 1                                                                   'делаем цикл для парсинга каждой ветви
            Dim xsub = xdef(xroot).Elements                                                         'получаем список элементов для парсинга
            Dim isub = xsub.Count                                                                   'получаем кол-во элементов
            For iroot = 0 To isub - 1                                                               'делаем цикл для парсинга каждой ноды (смотрим Name для имени ноды и Value для значения этой ноды)
                ListBox4.Items.Add("<" + xsub(iroot).Name.ToString + ">" + xsub(iroot).Value.ToString + "</" + xsub(iroot).Name.ToString + ">")
            Next
        Next


        Return (True)
    End Function

    Public Function FillSubDefs() As Boolean
        Dim i As Integer = ListBox2.SelectedIndex
        Dim PathSubDefs As String = ModNames(ListBox1.SelectedIndex).Item2 + "\Defs\" + DefsList(i)
        ListBox3.Items.Clear()

        SubDefsList = IO.Directory.GetFiles(PathSubDefs, "*.xml")
        Dim tmplist As String() = SubDefsList
        For sel = 0 To tmplist.Count - 1
            tmplist(sel) = tmplist.ElementAt(sel).Substring(PathSubDefs.Length + 1)
        Next

        ListBox3.Items.AddRange(tmplist)
        Return (True)
    End Function


    Public Function FillDefs() As Boolean
        Dim i As Integer = ListBox1.SelectedIndex
        Dim PathDefs As String = ModNames(i).Item2 + "\Defs"
        ListBox2.Items.Clear()

        If IO.Directory.Exists(PathDefs) Then
            DefsList = IO.Directory.GetDirectories(PathDefs)
            Dim tmplist As String() = DefsList
            For sel = 0 To tmplist.Count - 1
                tmplist(sel) = tmplist.ElementAt(sel).Substring(PathDefs.Length + 1)
            Next
            ListBox2.Items.AddRange(tmplist)
        End If
        Return (True)
    End Function


    Public Function FillNameMod() As Boolean
        Dim zzz As Integer = 0
        Dim CountSteamMods As Integer = 0
        Dim CountLocMods As Integer = 0
        Dim CountWorkMods As Integer = 0



        If SteamRimEnable Then
            Dim ListMods = My.Computer.FileSystem.GetDirectories(PathSteamRim + "\Mods")
            Dim WListMods = My.Computer.FileSystem.GetDirectories(PathSteamRimWorkshop)
            CountSteamMods = ListMods.Count
            CountWorkMods = WListMods.Count
            ListBox1.Items.Clear()
            For i = 0 To CountSteamMods - 1
                ModNames(i).Item1 = NameMod(ListMods(i))
                ModNames(i).Item2 = ListMods(i)
                ListBox1.Items.Add(ModNames(i).Item1)
            Next

            For i = 0 To CountWorkMods - 1
                ModNames(i + CountSteamMods).Item1 = NameMod(WListMods(i))
                ModNames(i + CountSteamMods).Item2 = WListMods(i)
                ListBox1.Items.Add(ModNames(i + CountSteamMods).Item1)
            Next

        End If

        Return (True)
    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
            Dim res As DialogResult = FolderBrowserDialog1.ShowDialog()
            If res = DialogResult.OK Then
                PathRim = FolderBrowserDialog1.SelectedPath
                If IO.File.Exists(PathRim + "\RimWorldWin64.exe") Or IO.File.Exists(PathRim + "\RimWorldWin.exe") Or IO.File.Exists(PathRim + "\RimWorld.exe") Then
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
                ComboBox1.Items.Clear()
                ComboBox1.Items.Add("English")
                ComboBox1.SelectedIndex = 0
                Return (False)

            End If

        End Function

        Public Function NameMod(FullPathMod As String) As String
            Dim ModName As String = ""
            Dim doc As XDocument = XDocument.Load(FullPathMod + "\About\About.xml")

        ModName = doc.Root.Element("name").Value.ToString

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
            RimLocModFullPath = IO.Directory.GetDirectories(CurModPath)
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
            FillNameMod()
        Else
            ListBox1.Items.Clear()
        End If

        ListBox1.Refresh()


        End Sub

        Private Sub ListBox1_SelectedValueChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedValueChanged
        Dim ModPath As String = ""
        Dim sel As Integer = 0
        sel = ListBox1.SelectedIndex
        TextBox2.Text = ModNames(sel).Item1 + vbCrLf + ModNames(sel).Item2
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        FillDefs()
    End Sub

    Private Sub ListBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox2.SelectedIndexChanged
        FillSubDefs()
    End Sub

    Private Sub ListBox3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox3.SelectedIndexChanged
        FillDefNames()
    End Sub
End Class
