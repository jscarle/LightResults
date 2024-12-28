BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.3374/23H2/2023Update/SunValley3)
13th Gen Intel Core i7-13700KF, 1 CPU, 24 logical and 16 physical cores
.NET SDK 9.0.100-preview.2.24157.14
[Host]   : .NET 8.0.3 (8.0.324.11423), X64 RyuJIT AVX2
.NET 8.0 : .NET 8.0.3 (8.0.324.11423), X64 RyuJIT AVX2

Job=.NET 8.0 Runtime=.NET 8.0 IterationTime=250.0000 ms
Iterations=10

| Method                                                      | Categories                                                                |         Mean |  Ratio | Allocated | Alloc Ratio |
|-------------------------------------------------------------|---------------------------------------------------------------------------|-------------:|-------:|----------:|------------:|
| A_LightResults_Result_Ok                                    | A01: Returning a successful result                                        |     2.567 ns |   1.00 |         - |          NA |
| B_FluentResults_Result_Ok                                   | A01: Returning a successful result                                        |   119.723 ns |  46.65 |     560 B |          NA |
| C_ArdalisResult_Result_Ok                                   | A01: Returning a successful result                                        |   277.651 ns | 108.18 |    3680 B |          NA |
| D_SimpleResults_Result_Ok                                   | A01: Returning a successful result                                        |   638.816 ns | 248.89 |     400 B |          NA |
| E_Rascal_Result_Ok                                          | A01: Returning a successful result                                        |     2.574 ns |   1.00 |         - |          NA |
|                                                             |                                                                           |              |        |           |             |
| A_LightResults_Result_OkTValue                              | A02: Returning a successful value result                                  |     2.661 ns |   1.00 |         - |          NA |
| B_FluentResults_Result_OkTValue                             | A02: Returning a successful value result                                  |   365.683 ns | 137.39 |    1200 B |          NA |
| C_ArdalisResult_Result_OkTValue                             | A02: Returning a successful value result                                  |   278.231 ns | 104.56 |    3680 B |          NA |
| D_SimpleResults_Result_OkTValue                             | A02: Returning a successful value result                                  |   650.526 ns | 244.46 |     480 B |          NA |
| E_Rascal_Result_OkTValue                                    | A02: Returning a successful value result                                  |     2.573 ns |   0.97 |         - |          NA |
|                                                             |                                                                           |              |        |           |             |
| A_LightResults_Result_Fail                                  | A03: Returning a failed result                                            |     2.559 ns |   1.00 |         - |          NA |
| B_FluentResults_Result_Fail                                 | A03: Returning a failed result                                            |   489.271 ns | 191.21 |    2640 B |          NA |
| C_ArdalisResult_Result_Fail                                 | A03: Returning a failed result                                            |   876.824 ns | 342.67 |    7040 B |          NA |
| D_SimpleResults_Result_Fail                                 | A03: Returning a failed result                                            |   665.515 ns | 260.09 |     400 B |          NA |
| E_Rascal_Result_Fail                                        | A03: Returning a failed result                                            |    20.893 ns |   8.16 |     240 B |          NA |
|                                                             |                                                                           |              |        |           |             |
| A_LightResults_Result_Fail_WithErrorMessage                 | A04: Returning a failed result with an error message                      |    25.014 ns |   1.00 |     320 B |        1.00 |
| B_FluentResults_Result_Fail_WithErrorMessage                | A04: Returning a failed result with an error message                      |   238.739 ns |   9.54 |    1120 B |        3.50 |
| C_ArdalisResult_Result_Fail_WithErrorMessage                | A04: Returning a failed result with an error message                      |   928.637 ns |  37.13 |    8000 B |       25.00 |
| D_SimpleResults_Result_Fail_WithErrorMessage                | A04: Returning a failed result with an error message                      |   338.846 ns |  13.55 |     400 B |        1.25 |
| E_Rascal_Result_Fail_WithErrorMessage                       | A04: Returning a failed result with an error message                      |     2.557 ns |   0.10 |         - |        0.00 |
|                                                             |                                                                           |              |        |           |             |
| A_LightResults_Result_FailTValue                            | A05: Returning a failed value result                                      |     2.597 ns |   1.00 |         - |          NA |
| B_FluentResults_Result_FailTValue                           | A05: Returning a failed value result                                      |   494.186 ns | 190.30 |    2720 B |          NA |
| C_ArdalisResult_Result_FailTValue                           | A05: Returning a failed value result                                      |   279.461 ns | 107.62 |    3680 B |          NA |
| D_SimpleResults_Result_FailTValue                           | A05: Returning a failed value result                                      |   995.774 ns | 383.41 |     880 B |          NA |
| E_Rascal_Result_FailTValue                                  | A05: Returning a failed value result                                      |    21.266 ns |   8.19 |     240 B |          NA |
|                                                             |                                                                           |              |        |           |             |
| A_LightResults_Result_FailTValue_WithErrorMessage           | A06: Returning a failed value result with an error message                |    22.537 ns |   1.00 |     320 B |        1.00 |
| B_FluentResults_Result_FailTValue_WithErrorMessage          | A06: Returning a failed value result with an error message                |   239.709 ns |  10.64 |    1200 B |        3.75 |
| C_ArdalisResult_Result_FailTValue_WithErrorMessage          | A06: Returning a failed value result with an error message                |   525.110 ns |  23.29 |    5680 B |       17.75 |
| D_SimpleResults_Result_FailTValue_WithErrorMessage          | A06: Returning a failed value result with an error message                |   684.760 ns |  30.39 |     880 B |        2.75 |
| E_Rascal_Result_FailTValue_WithErrorMessage                 | A06: Returning a failed value result with an error message                |     2.551 ns |   0.11 |         - |        0.00 |
|                                                             |                                                                           |              |        |           |             |
| A_LightResults_Result_Ok_ToString                           | B01: String representation of a successful result                         |     2.713 ns |   1.00 |         - |          NA |
| B_FluentResults_Result_Ok_ToString                          | B01: String representation of a successful result                         |   688.553 ns | 253.78 |    1520 B |          NA |
| C_ArdalisResult_Result_Ok_ToString                          | B01: String representation of a successful result                         |    18.577 ns |   6.85 |         - |          NA |
| D_SimpleResults_Result_Ok_ToString                          | B01: String representation of a successful result                         |    18.284 ns |   6.74 |         - |          NA |
| E_Rascal_Result_Ok_ToString                                 | B01: String representation of a successful result                         |   219.600 ns |  80.94 |     480 B |          NA |
|                                                             |                                                                           |              |        |           |             |
| A_LightResults_Result_OkTValue_ToString                     | B02: String representation of a successful value result                   |    94.202 ns |   1.00 |    1040 B |        1.00 |
| B_FluentResults_Result_OkTValue_ToString                    | B02: String representation of a successful value result                   |   949.433 ns |  10.08 |    3120 B |        3.00 |
| C_ArdalisResult_Result_OkTValue_ToString                    | B02: String representation of a successful value result                   |    18.344 ns |   0.19 |         - |        0.00 |
| D_SimpleResults_Result_OkTValue_ToString                    | B02: String representation of a successful value result                   |    18.346 ns |   0.19 |         - |        0.00 |
| E_Rascal_Result_OkTValue_ToString                           | B02: String representation of a successful value result                   |   212.399 ns |   2.25 |     400 B |        0.38 |
|                                                             |                                                                           |              |        |           |             |
| A_LightResults_Result_Fail_ToString                         | B03: String representation of a failed result                             |     5.481 ns |   1.00 |         - |          NA |
| B_FluentResults_Result_Fail_ToString                        | B03: String representation of a failed result                             | 1,725.632 ns | 314.84 |    3920 B |          NA |
| C_ArdalisResult_Result_Fail_ToString                        | B03: String representation of a failed result                             |    18.440 ns |   3.36 |         - |          NA |
| D_SimpleResults_Result_Fail_ToString                        | B03: String representation of a failed result                             |    18.643 ns |   3.40 |         - |          NA |
| E_Rascal_Result_Fail_ToString                               | B03: String representation of a failed result                             |   228.379 ns |  41.68 |     480 B |          NA |
|                                                             |                                                                           |              |        |           |             |
| A_LightResults_Result_Fail_WithErrorMessage_ToString        | B04: String representation of a failed result with an error message       |    90.450 ns |   1.00 |    1520 B |        1.00 |
| B_FluentResults_Result_Fail_WithErrorMessage_ToString       | B04: String representation of a failed result with an error message       | 2,275.817 ns |  25.16 |    9360 B |        6.16 |
| C_ArdalisResult_Result_Fail_WithErrorMessage_ToString       | B04: String representation of a failed result with an error message       |    18.475 ns |   0.20 |         - |        0.00 |
| D_SimpleResults_Result_Fail_WithErrorMessage_ToString       | B04: String representation of a failed result with an error message       |    18.462 ns |   0.20 |         - |        0.00 |
| E_Rascal_Result_Fail_WithErrorMessage_ToString              | B04: String representation of a failed result with an error message       |   266.069 ns |   2.94 |     960 B |        0.63 |
|                                                             |                                                                           |              |        |           |             |
| A_LightResults_Result_FailTValue_ToString                   | B05: String representation of a failed value result                       |     5.463 ns |   1.00 |         - |          NA |
| B_FluentResults_Result_FailTValue_ToString                  | B05: String representation of a failed value result                       | 2,055.815 ns | 376.39 |    5840 B |          NA |
| C_ArdalisResult_Result_FailTValue_ToString                  | B05: String representation of a failed value result                       |    18.689 ns |   3.42 |         - |          NA |
| D_SimpleResults_Result_FailTValue_ToString                  | B05: String representation of a failed value result                       |    18.772 ns |   3.44 |         - |          NA |
| E_Rascal_Result_FailTValue_ToString                         | B05: String representation of a failed value result                       |   231.157 ns |  42.32 |     480 B |          NA |
|                                                             |                                                                           |              |        |           |             |
| A_LightResults_Result_FailTValue_WithErrorMessage_ToString  | B06: String representation of a failed value result with an error message |    91.437 ns |   1.00 |    1520 B |        1.00 |
| B_FluentResults_Result_FailTValue_WithErrorMessage_ToString | B06: String representation of a failed value result with an error message | 2,749.966 ns |  30.08 |   12160 B |        8.00 |
| C_ArdalisResult_Result_FailTValue_WithErrorMessage_ToString | B06: String representation of a failed value result with an error message |    18.556 ns |   0.20 |         - |        0.00 |
| D_SimpleResults_Result_FailTValue_WithErrorMessage_ToString | B06: String representation of a failed value result with an error message |    18.287 ns |   0.20 |         - |        0.00 |
| E_Rascal_Result_FailTValue_WithErrorMessage_ToString        | B06: String representation of a failed value result with an error message |   248.487 ns |   2.72 |     960 B |        0.63 |
|                                                             |                                                                           |              |        |           |             |
| A_LightResults_Result_IsSuccess                             | C01: Determining if a result is successful                                |     2.568 ns |   1.00 |         - |          NA |
| B_FluentResults_Result_IsSuccess                            | C01: Determining if a result is successful                                |   236.091 ns |  91.93 |     560 B |          NA |
| C_ArdalisResult_Result_IsSuccess                            | C01: Determining if a result is successful                                |     2.452 ns |   0.95 |         - |          NA |
| D_SimpleResults_Result_IsSuccess                            | C01: Determining if a result is successful                                |     2.352 ns |   0.92 |         - |          NA |
| E_Rascal_Result_IsSuccess                                   | C01: Determining if a result is successful                                |     2.568 ns |   1.00 |         - |          NA |
|                                                             |                                                                           |              |        |           |             |
| A_LightResults_Result_Value                                 | C02: Retrieving the value                                                 |     2.442 ns |   1.00 |         - |          NA |
| B_FluentResults_Result_Value                                | C02: Retrieving the value                                                 |   248.593 ns | 101.81 |     560 B |          NA |
| C_ArdalisResult_Result_Value                                | C02: Retrieving the value                                                 |     2.351 ns |   0.96 |         - |          NA |
| D_SimpleResults_Result_Value                                | C02: Retrieving the value                                                 |     2.588 ns |   1.06 |         - |          NA |
| E_Rascal_Result_Value                                       | C02: Retrieving the value                                                 |     2.662 ns |   1.09 |         - |          NA |
|                                                             |                                                                           |              |        |           |             |
| A_LightResults_Result_IsFailed                              | C03: Determining if a result is failed                                    |     2.589 ns |   1.00 |         - |          NA |
| B_FluentResults_Result_IsFailed                             | C03: Determining if a result is failed                                    |   301.690 ns | 116.48 |     960 B |          NA |
| C_ArdalisResult_Result_IsFailed                             | C03: Determining if a result is failed                                    |     2.541 ns |   0.98 |         - |          NA |
| D_SimpleResults_Result_IsFailed                             | C03: Determining if a result is failed                                    |     2.581 ns |   1.00 |         - |          NA |
| E_Rascal_Result_IsFailed                                    | C03: Determining if a result is failed                                    |     2.607 ns |   1.01 |         - |          NA |
|                                                             |                                                                           |              |        |           |             |
| A_LightResults_Result_HasError                              | C04: Determining if a result contains a specific error                    |     9.793 ns |   1.00 |         - |          NA |
| B_FluentResults_Result_HasError                             | C04: Determining if a result contains a specific error                    | 1,307.727 ns | 133.54 |    4240 B |          NA |
| C_ArdalisResult_Result_HasError                             | C04: Determining if a result contains a specific error                    |   115.267 ns |  11.77 |     400 B |          NA |
| D_SimpleResults_Result_HasError                             | C04: Determining if a result contains a specific error                    |     2.603 ns |   0.27 |         - |          NA |
| E_Rascal_Result_HasError                                    | C04: Determining if a result contains a specific error                    |    10.530 ns |   1.08 |         - |          NA |
|                                                             |                                                                           |              |        |           |             |
| A_LightResults_Result_FirstError                            | C05: Retrieving the first error                                           |     2.599 ns |   1.00 |         - |          NA |
| B_FluentResults_Result_FirstError                           | C05: Retrieving the first error                                           |   490.054 ns | 188.58 |    1840 B |          NA |
| C_ArdalisResult_Result_FirstError                           | C05: Retrieving the first error                                           |    99.470 ns |  38.28 |         - |          NA |
| D_SimpleResults_Result_FirstError                           | C05: Retrieving the first error                                           |    32.582 ns |  12.54 |         - |          NA |
| E_Rascal_Result_FirstError                                  | C05: Retrieving the first error                                           |    11.795 ns |   4.54 |         - |          NA |

| Method                                              |       Mean | Allocated |
|-----------------------------------------------------|-----------:|----------:|
| Current_Result_Ok                                   |   2.719 ns |         - |
| Current_Result_Ok_ToString                          |   2.867 ns |         - |
| Current_Result_Fail                                 |   2.569 ns |         - |
| Current_Result_Fail_ToString                        |   6.028 ns |         - |
| Current_Result_Fail_WithErrorMessage                |  49.422 ns |     560 B |
| Current_Result_Fail_WithErrorMessage_ToString       | 287.388 ns |    2480 B |
| Current_Result_OkTValue                             |  37.276 ns |     320 B |
| Current_Result_OkTValue_ToString                    | 253.089 ns |    1520 B |
| Current_Result_FailTValue                           |   2.440 ns |         - |
| Current_Result_FailTValue_ToString                  |   6.106 ns |         - |
| Current_Result_FailTValue_WithErrorMessage          |  49.910 ns |     640 B |
| Current_Result_FailTValue_WithErrorMessage_ToString | 251.820 ns |    2480 B |
| Current_Result_HasError                             |   9.633 ns |         - |
| Current_Result_Error                                |   3.293 ns |         - |
| Current_Error_New                                   |  31.986 ns |     320 B |
| Current_Error_New_ToString                          |  36.593 ns |         - |
| Current_Error_New_WithErrorMessage                  |  31.509 ns |     320 B |
| Current_Error_New_WithErrorMessage_ToString         | 212.182 ns |    1200 B |

| Method                                              |       Mean | Allocated |
|-----------------------------------------------------|-----------:|----------:|
| Develop_Result_Ok                                   |   2.647 ns |         - |
| Develop_Result_Ok_ToString                          |   2.732 ns |         - |
| Develop_Result_Fail                                 |   2.377 ns |         - |
| Develop_Result_Fail_ToString                        |   5.485 ns |         - |
| Develop_Result_Fail_WithErrorMessage                |  25.435 ns |     320 B |
| Develop_Result_Fail_WithErrorMessage_ToString       |  90.408 ns |    1520 B |
| Develop_Result_OkTValue                             |   2.489 ns |         - |
| Develop_Result_OkTValue_ToString                    |  91.808 ns |    1040 B |
| Develop_Result_FailTValue                           |   2.500 ns |         - |
| Develop_Result_FailTValue_ToString                  |   5.450 ns |         - |
| Develop_Result_FailTValue_WithErrorMessage          |  22.273 ns |     320 B |
| Develop_Result_FailTValue_WithErrorMessage_ToString |  90.424 ns |    1520 B |
| Develop_Result_HasError                             |   9.708 ns |         - |
| Develop_Result_Error                                |   2.616 ns |         - |
| Develop_Error_New                                   |  31.918 ns |     320 B |
| Develop_Error_New_ToString                          |  42.013 ns |         - |
| Develop_Error_New_WithErrorMessage                  |  32.300 ns |     320 B |
| Develop_Error_New_WithErrorMessage_ToString         | 140.613 ns |    1200 B |
