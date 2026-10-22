# Test Your Luck

다양한 운 기반 미니게임을 플레이하고, 획득한 골드로 기록과 순위를 경쟁하는 Unity 모바일 게임 프로젝트입니다.

## 주요 기능

- 코인 뒤집기(Coin Flip) 및 가위바위보 미니게임
- 보유 골드를 사용하는 베팅 시스템
- Google 로그인 및 Firebase Authentication 연동
- Firebase Realtime Database 기반 골드 저장과 리더보드
- Firebase Firestore 기반 24시간 간격 일일 보상
- 배경음악·효과음 볼륨 설정 및 모바일 화면 대응

## 개발 환경

- Unity `6000.0.32f1`
- Universal Render Pipeline `17.0.3`
- Input System `1.11.2`
- TextMesh Pro / Unity UI
- Firebase Authentication, Realtime Database, Firestore
- Google Sign-In

## 주요 씬

| 씬 | 설명 |
| --- | --- |
| `MainWindow` | Google 로그인 및 게임 시작 화면 |
| `GameChoose` | 미니게임 선택, 골드 및 리더보드 UI |
| `CoinFlip` | 코인 뒤집기와 베팅 진행 |
| `RockPaperScissors` | 가위바위보 선택, 결과 판정 및 보상 처리 |

빌드 씬 순서는 `MainWindow` → `GameChoose` → `CoinFlip` → `RockPaperScissors`로 설정되어 있습니다.

## 실행 방법

1. Git LFS를 설치합니다.
2. 저장소를 클론한 뒤 LFS 에셋을 내려받습니다.

   ```bash
   git clone https://github.com/sungmin2ee/TestYourLuck.git
   cd TestYourLuck
   git lfs pull
   ```

3. Unity Hub에서 프로젝트를 열고 Unity `6000.0.32f1`을 사용합니다.
4. Firebase 및 Google Sign-In 설정이 대상 플랫폼과 일치하는지 확인합니다.
5. `Assets/Scenes/MainWindow.unity`를 열어 실행합니다.

## 프로젝트 구조

```text
Assets/          게임 씬, 스크립트, 프리팹, UI 및 오디오 에셋
Packages/        Unity 패키지 의존성
ProjectSettings/ Unity 프로젝트 및 빌드 설정
```

`Library`, `Temp`, `Logs`, `build` 등 Unity가 자동 생성하는 폴더는 버전 관리에서 제외됩니다. 대용량 이미지, 오디오 및 네이티브 플러그인은 Git LFS로 관리합니다.
