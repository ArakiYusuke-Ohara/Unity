@echo off
chcp 65001 >nul
set /p "target_dir=削除したいディレクトリのパスを入力してください: "

if not exist "%target_dir%" (
    echo 指定したパスが存在しません。
    pause
    exit /b
)

for /d /r "%target_dir%" %%d in (.vs) do (
    if exist "%%d" (
        rd /s /q "%%d"
        echo "%%d フォルダを削除しました。"
    )
)

for /d /r "%target_dir%" %%d in (Library) do (
    if exist "%%d" (
        rd /s /q "%%d"
        echo "%%d フォルダを削除しました。"
    )
)

for /d /r "%target_dir%" %%d in (Temp) do (
    if exist "%%d" (
        rd /s /q "%%d"
        echo "%%d フォルダを削除しました。"
    )
)

for /d /r "%target_dir%" %%d in (Logs) do (
    if exist "%%d" (
        rd /s /q "%%d"
        echo "%%d フォルダを削除しました。"
    )
)

for /d /r "%target_dir%" %%d in (Obj) do (
    if exist "%%d" (
        rd /s /q "%%d"
        echo "%%d フォルダを削除しました。"
    )
)

for /d /r "%target_dir%" %%d in (UserSettings) do (
    if exist "%%d" (
        rd /s /q "%%d"
        echo "%%d フォルダを削除しました。"
    )
)

for /d /r "%target_dir%" %%d in (MemoryCaptures) do (
    if exist "%%d" (
        rd /s /q "%%d"
        echo "%%d フォルダを削除しました。"
    )
)

echo "完了しました。"
pause
