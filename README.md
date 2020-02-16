# Boot Run （注册Windows管理员权限启动应用）

  通过API修改Windows任务计划程序，实现Windows应用以管理员权限开机自启。

## 使用方法

```bash
boot-run.exe "AppName" "ExeFilePath" "Arguments"
```

**注意：** 该程序需以管理员权限启动。

## Example

```bash
boot-run.exe "NotePad" "C:\Windows\System32\notepad.exe" "C:\test.log"
```

## 相关博文

[实现Windows应用以管理员权限开机自启](https://www.wyr.me/post/615)
