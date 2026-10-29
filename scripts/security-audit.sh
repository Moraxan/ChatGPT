#!/usr/bin/env bash
# security-audit.sh - audit every package dir in this repo for vulnerable deps.
# Exit 1 if any vulnerability >= AUDIT_LEVEL (default: critical) is found.
# AUDIT_ALL=1 also audits devDependencies (default: production only, --omit=dev).
# No npm install needed: npm audit reads package-lock.json directly.
set -u
LEVEL="${AUDIT_LEVEL:-critical}"
cd "$(dirname "$0")/.." || exit 2
OMIT="--omit=dev"
[ "${AUDIT_ALL:-0}" = "1" ] && OMIT=""
FAIL=0
TOTAL=0
while IFS= read -r -d '' lock; do
  dir=$(dirname "$lock")
  TOTAL=$((TOTAL+1))
  if out=$(cd "$dir" && npm audit $OMIT --audit-level="$LEVEL" 2>&1); then
    echo "OK    $dir"
  else
    echo "FAIL  $dir  (vulns >= $LEVEL)"
    printf '%s\n' "$out" | grep -E '^[0-9]+ (critical|high|moderate|low)' | head -5 | sed 's/^/      /'
    FAIL=1
  fi
done < <(find . -name node_modules -prune -o -name .git -prune -o -name package-lock.json -print0)
echo "----"
echo "audited $TOTAL package dir(s), threshold=$LEVEL, prod-only=$([ -n "$OMIT" ] && echo yes || echo no) => $([ $FAIL -eq 0 ] && echo PASS || echo FAIL)"
exit $FAIL
