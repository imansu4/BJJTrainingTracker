# Manual Testing Guide

Run these checks in Visual Studio before the live presentation.

| Area | Test | Expected result |
| --- | --- | --- |
| Session validation | Leave Techniques empty and select Save Session | A warning appears and no session is added |
| Session storage | Add a valid session, close the program and open it again | The saved session returns in the session list |
| Session editing | Select a session, change its duration and select Update Selected | The list shows the new duration and the change remains after restart |
| Session deletion | Select a session and confirm deletion | The session is removed and stays removed after restart |
| Search and filters | Search for a technique and combine type and date filters | Only matching sessions appear and the match count updates |
| Invalid date range | Set From date later than To date | The list is empty and an explanatory message appears |
| Weekly summary | Select dates with sessions and a week with no sessions | Totals match the selected Monday-to-Sunday week and zero values display correctly |
| Focus-area validation | Try to add a blank or duplicate focus area | The program rejects the entry with a clear message |
| Focus-area workflow | Add, complete, reopen and delete a focus area | The item moves between the correct lists and deletion requires confirmation |
| Focus-area storage | Add and complete focus areas, then restart the program | Active and completed states are restored from JSON |
| Damaged data | Temporarily replace a data file with invalid JSON | The program shows a data warning instead of closing unexpectedly |

The generated data files are stored in the application's output `Data` folder. Restore or remove any deliberately damaged test file after testing.
