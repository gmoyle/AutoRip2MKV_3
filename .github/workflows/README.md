# GitHub Actions Workflows

This directory contains GitHub Actions workflows for the AutoRip2MKV project.

## Workflows

### 1. `build-test.yml` - Build and Test
**Triggers:** All pushes and pull requests  
**Purpose:** Quick build and test validation  
**Features:**
- Builds the solution on Windows runners
- Runs unit tests
- Creates build artifacts for master/main branches
- Lightweight and fast feedback

### 2. `ci.yml` - Full CI/CD Pipeline  
**Triggers:** Pushes to master/main, pull requests, manual dispatch  
**Purpose:** Comprehensive continuous integration  
**Features:**
- **Build and Test Job:**
  - Full solution build
  - Unit test execution with coverage
  - Test results and coverage uploads
  - Codecov integration
  
- **Release Artifacts Job:**
  - Self-contained executable creation
  - Single-file publishing
  - Release package generation
  - Artifact retention (30 days)
  
- **Code Quality Job:**
  - SonarCloud integration (optional)
  - Static code analysis
  - Quality gate validation
  
- **Security Scan Job:**
  - Vulnerability scanning
  - Package audit
  - Security analysis

## Usage

### Manual Workflow Trigger
You can manually trigger workflows from the GitHub Actions tab:
1. Go to your repository on GitHub
2. Click "Actions" tab
3. Select the workflow you want to run
4. Click "Run workflow"

### Download Build Artifacts
After successful builds, you can download artifacts:
1. Go to the completed workflow run
2. Scroll to "Artifacts" section
3. Download the `AutoRip2MKV-Build` or `AutoRip2MKV-Release` package

### Setting Up SonarCloud (Optional)
To enable code quality analysis:
1. Sign up at [SonarCloud](https://sonarcloud.io)
2. Import your repository
3. Add `SONAR_TOKEN` to your repository secrets
4. The workflow will automatically include SonarCloud analysis

## Requirements

- **.NET 6.0**: All workflows use .NET 6.0 runtime
- **Windows Runners**: Required for Windows Forms application
- **Repository Secrets**: Optional for SonarCloud integration

## Artifacts

### Build Artifacts (`AutoRip2MKV-Build`)
- Framework-dependent deployment
- Requires .NET 6.0 runtime on target machine
- Smaller download size
- Retention: 7 days

### Release Artifacts (`AutoRip2MKV-Release`)
- Self-contained deployment
- Includes .NET runtime
- Single executable file
- Larger download but no dependencies required
- Retention: 30 days

## Troubleshooting

### Build Failures
- Check that all project references are correct
- Ensure NuGet package versions are compatible
- Verify .NET 6.0 target framework

### Test Failures
- Review test output in the workflow logs
- Check for missing dependencies
- Verify test project configuration

### Missing Artifacts
- Artifacts are only created for master/main branch builds
- Check if the build completed successfully
- Verify retention period hasn't expired
